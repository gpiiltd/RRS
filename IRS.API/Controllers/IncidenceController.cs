using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.Models.QueryResources.Incidence;
using IRS.DAL.Models.QueryResource.Incidence;
using IRS.DAL.Models;
using IRS.API.Dtos.IncidenceResources;
using Microsoft.AspNetCore.Authorization;
using IRS.DAL.ViewModel;
using IRS.DAL.Models.Shared;
using IRS.API.Helpers.Abstract;
using System.IO;
using IRS.API.Dtos;
using Microsoft.AspNetCore.Hosting;

namespace IRS.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    //[Authorize] --uncomment to protect route with token auth
    public class IncidenceController : ControllerBase
    {
        private readonly IMapper  mapper;
        private readonly IIncidenceRepository repository;
        private readonly IUnitofWork unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly INotificationRepository _notificationRepo;
        private readonly ISettingsRepository _settingsRepo;
        private readonly IHostingEnvironment _host;
        private readonly IPhotoService _photoService;
        public IIncidenceTypeDepartmentRepository _incidenceTypeDeptRepo;

        public IncidenceController(IMapper mapper, IIncidenceRepository repository, IUnitofWork unitOfWork, IUserRepository userRepo, INotificationRepository notificationRepo,
            ISettingsRepository settingsRepo, IHostingEnvironment host, IPhotoService photoService, IIncidenceTypeDepartmentRepository incidenceTypeDeptRepo)
        {
            _host = host;
            this.mapper = mapper;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            _userRepo = userRepo;
            _notificationRepo = notificationRepo;
            _settingsRepo = settingsRepo;
            _photoService = photoService;
            _incidenceTypeDeptRepo = incidenceTypeDeptRepo;
        }

        //GET: api/incidence
        [HttpGet]
        [Route("getIncidenceList")]
        public async Task<QueryResultResource<IncidenceResource>> GetIncidenceList(IncidenceQueryResource filterResource)
        {
            var filter = mapper.Map<IncidenceQueryResource, IncidenceQuery>(filterResource);
            var queryResult = await repository.GetIncidences(filter);

            return mapper.Map<QueryResult<IncidenceViewModel>, QueryResultResource<IncidenceResource>>(queryResult);
        }

        // GET: api/incidence/5
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetIncidence([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var incidences = await repository.GetIncidence(id);
            
            if (incidences == null)
            {
                return NotFound();
            }
             var incidenceResource = mapper.Map<Incidence, IncidenceResource>(incidences);
            return Ok(incidenceResource);
        }

        // PUT: api/incidence/5
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutIncidence([FromRoute] Guid id, [FromBody] SaveIncidenceResource incidenceResource)
        {
            try
            {
                bool triggerNotification = false;
                //input validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var incidence = await repository.GetIncidence(id);
                if (incidence == null)
                {
                    return NotFound();
                }
            
                incidenceResource.EditedByUserId = _userRepo.GetLoggedInUserId();
                incidenceResource.DateCreated = incidence.DateCreated;
                incidenceResource.CreatedByUserId = incidence.CreatedByUserId;
                incidenceResource.Code = incidence.Code;
                incidenceResource.DateEdited = DateTime.UtcNow;
                incidenceResource.AssignerId = incidence.AssignerId;
                incidenceResource.AssignedOrganizationId = incidence.AssignedOrganizationId;
                incidenceResource.ResolutionDate = incidence.ResolutionDate;

                incidenceResource.Description = incidence.Description;
                incidenceResource.ReportedIncidenceLatitude = incidence.ReportedIncidenceLatitude ?? 0;
                incidenceResource.ReportedIncidenceLongitude = incidence.ReportedIncidenceLongitude ?? 0;
                incidenceResource.ReporterName = incidence.ReporterName;
                incidenceResource.ReporterEmail = incidence.ReporterEmail;
                incidenceResource.ReporterFirstResponderAction = incidence.ReporterFirstResponderAction;
                incidenceResource.ReporterFeedbackRating = incidence.ReporterFeedbackRating;
                incidenceResource.Protected = incidence.Protected;
                incidenceResource.Deleted = incidence.Deleted;

                // trigger alert when incidence status is open or re-opened
                if (incidenceResource.IncidenceStatusId != incidence.IncidenceStatusId && (incidenceResource.IncidenceStatusId == GlobalFields.OpenIncidenceStatus || incidenceResource.IncidenceStatusId == GlobalFields.ReOpenedIncidenceStatus))
                {
                    incidenceResource.AssignerId = _userRepo.GetLoggedInUserId();
                    triggerNotification = true;
                }
                else if (incidenceResource.IncidenceStatusId != incidence.IncidenceStatusId && incidenceResource.IncidenceStatusId == GlobalFields.ClosedIncidenceStatus)
                {
                    // set resolution date when incidence is closed
                    incidenceResource.ResolutionDate = DateTime.Now;
                    triggerNotification = true;
                }
                else if (incidenceResource.IncidenceStatusId != incidence.IncidenceStatusId && (incidenceResource.IncidenceStatusId == GlobalFields.ResolvedIncidenceStatus || incidenceResource.IncidenceStatusId == GlobalFields.UnderReviewIncidenceStatus))
                {
                    triggerNotification = true;
                }

                mapper.Map<SaveIncidenceResource, Incidence>(incidenceResource, incidence);
            
                await unitOfWork.CompleteAsync();

                //send email and sms to notify assignee upon incidence opening and assignment
                //IncidenceType's mapped department's settings is used for sending notification, otherwise if empty, use organization or system settings 
                if (triggerNotification)
                    await _notificationRepo.SendIncidenceNotification(incidenceResource, incidenceResource.AssignedDepartmentId, incidenceResource.AssignerId, incidenceResource.AssigneeId, incidenceResource.AssignedOrganizationId);

                incidence = await repository.GetIncidence(id);
                var result = mapper.Map<Incidence, IncidenceResource>(incidence);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        // POST: api/incidence - To submit incidence from web from minus photos/videos
        [HttpPost]
        [Route("createIncidence")]
        public async Task<IActionResult> CreateIncidence([FromBody] SaveIncidenceResource incidenceResource)
        {
            try
            {
                var loggedInUserId = _userRepo.GetLoggedInUserId();
                var user = await _userRepo.GetUser(loggedInUserId);
                bool triggerNotification = false;
                bool triggerDeptNotification = false;
                //assumes the organization creates this incidence front front-end, notification applies only when its assigned, resolved or closed
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //business rule validation
                var incidenceCode = !string.IsNullOrEmpty(incidenceResource.Description) ? incidenceResource.Description + "/" 
                    + ((incidenceResource.ReportedIncidenceLatitude != null) ? incidenceResource.ReportedIncidenceLatitude : DateTime.Now.Minute) + "/" 
                    + ((incidenceResource.ReportedIncidenceLongitude != null) ? incidenceResource.ReportedIncidenceLongitude : DateTime.Now.Minute)
                    : incidenceResource.ReportedIncidenceLatitude + "/" + incidenceResource.ReportedIncidenceLongitude;
                if (repository.CheckIncidenceCode(incidenceCode))
                {
                    ModelState.AddModelError("Error", "The incidence with this Code already exists.");
                    return BadRequest(ModelState);
                }

                var incidence= mapper.Map<SaveIncidenceResource, Incidence>(incidenceResource);
                incidence.Deleted = false;
                // incidence.Protected = true; in other to allow pseudo-deletion which only hides the record
                incidence.CreatedByUserId = _userRepo.GetLoggedInUserId();
                incidence.DateCreated = DateTime.Now;
                incidence.IncidenceStatusId = incidenceResource.IncidenceStatusId ?? GlobalFields.NewIncidenceStatus;
                incidence.Code = incidenceCode;
                //incidence.LastUpdate = DateTime.Now;

                if (user != null)
                {
                    var reporterFullName = string.IsNullOrEmpty(user.MiddleName) ? user.FirstName + " " + user.LastName
                        : user.FirstName + " " + user.MiddleName + " " + user.LastName;
                    incidence.ReporterName = reporterFullName ?? "";
                    incidence.ReporterDepartmentId = user.DepartmentId;
                    incidence.ReporterEmail = user.Email1;
                }
                incidence.OrganizationId = user.UserName != "Admin" ? user.OrganizationId : null;
                // trigger alert when incidence status is open or re-opened provided as Assignee and Assigner were sent
                if (incidenceResource.AssigneeId != null && (incidenceResource.IncidenceStatusId == GlobalFields.OpenIncidenceStatus || incidenceResource.IncidenceStatusId == GlobalFields.ReOpenedIncidenceStatus))
                {
                    incidence.AssignerId = loggedInUserId;
                    triggerNotification = true;
                }
                else if (incidenceResource.IncidenceStatusId != incidence.IncidenceStatusId && incidenceResource.IncidenceStatusId == GlobalFields.ClosedIncidenceStatus)
                {
                    // set resolution date when incidence is closed. No notification here as it incidence was already closed before creation. No Assigner or Assignee before now
                    incidenceResource.ResolutionDate = DateTime.Now;
                }

                var incidenceTypeDepartment = new OrganizationDepartment();
                if (incidenceResource.IncidenceTypeId != null)
                    incidenceTypeDepartment = await _incidenceTypeDeptRepo.GetDepartmentFromIncidenceType(incidenceResource.IncidenceTypeId);
                incidence.IncidenceStatusId = incidenceResource.IncidenceStatusId ?? GlobalFields.NewIncidenceStatus;
                if (incidence.IncidenceStatusId == GlobalFields.NewIncidenceStatus && incidenceResource.IncidenceTypeId != null)
                {
                    incidence.AssignedDepartmentId = incidenceTypeDepartment.Id;
                    incidence.OrganizationId = incidenceTypeDepartment.OrganizationId;
                    incidence.AssignedOrganizationId = incidenceTypeDepartment.OrganizationId;
                    triggerDeptNotification = true;
                }

                repository.Add(incidence);
                await unitOfWork.CompleteAsync();

                if (triggerDeptNotification)
                {
                    //pass the reporter details into incidenceResource as it only exists in incidence and it is needed in email notification
                    incidenceResource.ReporterName = incidence.ReporterName;
                    incidenceResource.ReporterEmail = incidence.ReporterEmail;
                    incidence.ReporterDepartmentId = incidence.ReporterDepartmentId;
                    await _notificationRepo.SendEmailNotificationToOrgDeptUsers(incidenceResource, incidenceTypeDepartment.Id, incidenceTypeDepartment.OrganizationId, incidence.ReporterDepartmentId); // use SendEmailNotificationToOrgDeptUsers method in notificationRepo
                }

                if (triggerNotification)
                    await _notificationRepo.SendIncidenceNotification(incidenceResource, incidenceResource.AssignedDepartmentId, incidenceResource.AssignerId, incidenceResource.AssigneeId, incidenceResource.AssignedOrganizationId);

                incidence = await repository.GetIncidence(incidence.Id);
                var result = mapper.Map<Incidence, IncidenceResource>(incidence);

                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }


        [HttpPost] //- To submit incidence from mobile app including single photo/video
        [Route("CreateIncidenceOnMobile")]
        public async Task<IActionResult> CreateIncidenceRemote([FromForm] SaveIncidenceResource incidenceResource)
        {
            try
            {
                var loggedInUserId = _userRepo.GetLoggedInUserId();
                var user = await _userRepo.GetUser(loggedInUserId);
                bool triggerNotification = false;
                bool triggerDeptNotification = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //business rule validation
                var incidenceCode = !string.IsNullOrEmpty(incidenceResource.Description) ? incidenceResource.Description + "/"
                    + ((incidenceResource.ReportedIncidenceLatitude != null) ? incidenceResource.ReportedIncidenceLatitude : DateTime.Now.Minute) + "/"
                    + ((incidenceResource.ReportedIncidenceLongitude != null) ? incidenceResource.ReportedIncidenceLongitude : DateTime.Now.Minute)
                    : incidenceResource.ReportedIncidenceLatitude + "/" + incidenceResource.ReportedIncidenceLongitude;
                if (repository.CheckIncidenceCode(incidenceCode))
                {
                    ModelState.AddModelError("Error", "The incidence with this Code already exists.");
                    return BadRequest(ModelState);
                }

                var incidence = mapper.Map<SaveIncidenceResource, Incidence>(incidenceResource);
                incidence.Deleted = false;
                incidence.CreatedByUserId = loggedInUserId;
                // incidence.Protected = true; in other to allow pseudo-deletion which only hides the record
                incidence.DateCreated = DateTime.UtcNow;
                incidence.IncidenceStatusId = GlobalFields.NewIncidenceStatus;
                incidence.Code = incidenceCode;
                //incidence.LastUpdate = DateTime.Now;

                if (user != null)
                {
                    var reporterFullName = string.IsNullOrEmpty(user.MiddleName) ? user.FirstName + " " + user.LastName
                        : user.FirstName + " " + user.MiddleName + " " + user.LastName;
                    incidence.ReporterName = reporterFullName ?? "" ;
                    incidence.ReporterDepartmentId = user.DepartmentId;
                    incidence.ReporterEmail = user.Email1;
                }

                // trigger alert when incidence status is open or re-opened provided as Assignee and Assigner were sent
                if (incidenceResource.AssigneeId != null && (incidenceResource.IncidenceStatusId == GlobalFields.OpenIncidenceStatus || incidenceResource.IncidenceStatusId == GlobalFields.ReOpenedIncidenceStatus))
                {
                    incidence.AssignerId = loggedInUserId;
                    triggerNotification = true;
                }

                var incidenceTypeDepartment = new OrganizationDepartment();
                if (incidenceResource.IncidenceTypeId != null)
                    incidenceTypeDepartment = await _incidenceTypeDeptRepo.GetDepartmentFromIncidenceType(incidenceResource.IncidenceTypeId);

                incidence.AssignedDepartmentId = incidenceTypeDepartment.Id;
                incidence.OrganizationId = incidenceTypeDepartment.OrganizationId;
                incidence.AssignedOrganizationId = incidenceTypeDepartment.OrganizationId;

                incidence.IncidenceStatusId = incidenceResource.IncidenceStatusId ?? GlobalFields.NewIncidenceStatus;
                if (incidence.IncidenceStatusId == GlobalFields.NewIncidenceStatus && incidenceResource.IncidenceTypeId != null)
                {
                    incidence.AssignedDepartmentId = incidenceTypeDepartment.Id;
                    incidence.OrganizationId = incidenceTypeDepartment.OrganizationId;
                    incidence.AssignedOrganizationId = incidenceTypeDepartment.OrganizationId;
                    triggerDeptNotification = true;
                }

                repository.Add(incidence);
                await unitOfWork.CompleteAsync();

                incidence = await repository.GetIncidence(incidence.Id);

                if (incidence == null)
                    return NotFound();

                if (incidenceResource.file != null && incidenceResource.file.Length != 0)
                {
                    var deptSettings = await _settingsRepo.GetDepartmentSettings(incidenceTypeDepartment.Id);
                    var orgSettings = await _settingsRepo.GetOrganizationSettings(incidenceTypeDepartment.OrganizationId);
                    var generalSettings = await _settingsRepo.GetGeneralSettings();

                    if (!_settingsRepo.ValidateFileType(deptSettings, orgSettings, generalSettings, incidenceResource.file))
                        return BadRequest("Invalid file type");

                    if (!_settingsRepo.ValidateFileSize(deptSettings, orgSettings, generalSettings, incidenceResource.file))
                        return BadRequest("Maximun file size exceeded");

                    var isFilePhotoNotVideo = _settingsRepo.IsImageFileNotVideo(deptSettings, orgSettings, generalSettings, incidenceResource.file);

                    //Dont use webrootpath i.e wwwroot as it gets cleared after angular ng build
                    var uploadsFolderPath = Path.Combine(_host.WebRootPath, "uploads");
                    var photo = await _photoService.UploadPhoto(incidence, incidenceResource.file, uploadsFolderPath, FileUploadChannels.incidencesReportedOnMobile, isFilePhotoNotVideo);

                    // return Ok(mapper.Map<Media, MediaResource>(photo));
                }

                //Next, fetch notification settings from the IncidenceType's-mapped department or the mapped department's organization if dept settings is empty. Dept receives notification here
                //IncidenceType's mapped department's settings is used for sending notification, otherwise if empty, use organization or system settings  
                //await _notificationRepo.SendIncidenceNotification(incidenceResource, incidenceTypeDepartment.Id, null, null, incidenceTypeDepartment.OrganizationId);

                if (triggerDeptNotification)
                {
                    //pass the reporter details into incidenceResource as it only exists in incidence and it is needed in email notification
                    incidenceResource.ReporterName = incidence.ReporterName;
                    incidenceResource.ReporterEmail = incidence.ReporterEmail;
                    incidence.ReporterDepartmentId = incidence.ReporterDepartmentId;
                    await _notificationRepo.SendEmailNotificationToOrgDeptUsers(incidenceResource, incidenceTypeDepartment.Id, incidenceTypeDepartment.OrganizationId, incidence.ReporterDepartmentId); // use SendEmailNotificationToOrgDeptUsers method in notificationRepo
                }

                if (triggerNotification)
                    await _notificationRepo.SendIncidenceNotification(incidenceResource, incidenceResource.AssignedDepartmentId, incidenceResource.AssignerId, incidenceResource.AssigneeId, incidenceResource.AssignedOrganizationId);

                incidence = await repository.GetIncidence(incidence.Id);
                var result = mapper.Map<Incidence, IncidenceResource>(incidence);

                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }

            //var result = mapper.Map<Incidence, IncidenceResource>(incidence);

            //return Ok(result);
        }

        // DELETE: api/incidence/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Deleteincidence([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var incidence = await repository.GetIncidence(id, includeRelated: false);

                if (incidence == null )
                {
                    return NotFound();
                }

                if (incidence.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be deleted");
                    return BadRequest(ModelState);
                }

                incidence.Deleted = true;
                await unitOfWork.CompleteAsync();

                return Ok(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("sendIncidenceReporterRating/{id}/{rating}")]
        public async Task<IActionResult> SendIncidenceReporterRating([FromRoute] Guid id, [FromRoute] int rating)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var incidence = await repository.GetIncidence(id, includeRelated: false);

                if (incidence == null)
                {
                    return NotFound();
                }

                incidence.ReporterFeedbackRating = rating;

                await unitOfWork.CompleteAsync();

                return Ok(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }
    }
}