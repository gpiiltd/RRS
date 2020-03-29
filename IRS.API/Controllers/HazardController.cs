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
//using IRS.DAL.Models.QueryResources.Incidence;
//using IRS.DAL.Models.QueryResource.Incidence;
using IRS.DAL.Models;
//using IRS.API.Dtos.IncidenceResources;
using Microsoft.AspNetCore.Authorization;
using IRS.DAL.ViewModel;
using IRS.DAL.Models.Shared;
using IRS.API.Helpers.Abstract;
using System.IO;
using IRS.API.Dtos;
using Microsoft.AspNetCore.Hosting;
using IRS.API.Dtos.HazardResources;
using IRS.DAL.Models.QueryResources.Hazard;
using IRS.DAL.Models.QueryResource.Hazard;

namespace IRS.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    //[Authorize] --uncomment to protect route with token auth
    public class HazardController : ControllerBase
    {
        private readonly IMapper  mapper;
        private readonly IHazardRepository repository;
        private readonly IUnitofWork unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly IOrganizationRepository _orgRepo;
        private readonly INotificationRepository _notificationRepo;
        private readonly ISettingsRepository _settingsRepo;
        private readonly IHostingEnvironment _host;
        private readonly IPhotoService _photoService;
        public IIncidenceTypeDepartmentRepository _incidenceTypeDeptRepo;

        public HazardController(IMapper mapper, IHazardRepository repository, IUnitofWork unitOfWork, IUserRepository userRepo, INotificationRepository notificationRepo,
            ISettingsRepository settingsRepo, IHostingEnvironment host, IPhotoService photoService, IIncidenceTypeDepartmentRepository incidenceTypeDeptRepo, IOrganizationRepository orgRepo)
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
            _orgRepo = orgRepo;
        }

        //GET: api/hazard
        [HttpGet]
        [Route("getHazardList")]
        public async Task<QueryResultResource<HazardResource>> GetHazardList(HazardQueryResource filterResource)
        {
            var filter = mapper.Map<HazardQueryResource, HazardQuery>(filterResource);
            var queryResult = await repository.GetHazards(filter);

            return mapper.Map<QueryResult<HazardViewModel>, QueryResultResource<HazardResource>>(queryResult);
        }

        // GET: api/incidence/5
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetHazard([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hazards = await repository.GetHazard(id);
            
            if (hazards == null)
            {
                return NotFound();
            }
             var hazardResource = mapper.Map<Hazard, HazardResource>(hazards);
            return Ok(hazardResource);
        }

        // PUT: api/incidence/5
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutHazard([FromRoute] Guid id, [FromBody] SaveHazardResource hazardResource)
        {
            try
            {
                bool triggerNotification = false;
                //input validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var hazard = await repository.GetHazard(id);
                if (hazard == null)
                {
                    return NotFound();
                }

                hazardResource.EditedByUserId = _userRepo.GetLoggedInUserId();
                hazardResource.DateCreated = hazard.DateCreated;
                hazardResource.CreatedByUserId = hazard.CreatedByUserId;

                hazardResource.Description = hazard.Description;
                hazardResource.ReportedLatitude = hazard.ReportedLatitude ?? 0;
                hazardResource.ReportedLongitude = hazard.ReportedLongitude ?? 0;
                hazardResource.ReporterName = hazard.ReporterName;
                hazardResource.ReporterEmail = hazard.ReporterEmail;
                hazardResource.ReporterFirstResponderAction = hazard.ReporterFirstResponderAction;
                hazardResource.ReporterFeedbackRating = hazard.ReporterFeedbackRating;
                //hazardResource.ManagerFeedbackRating = hazard.ManagerFeedbackRating;
                hazardResource.Protected = hazard.Protected;
                hazardResource.Deleted = hazard.Deleted;

                hazardResource.Code = hazard.Code;
                hazardResource.DateEdited = DateTime.UtcNow;
                hazardResource.Deleted = hazard.Deleted;
                //incidenceResource.Protected = incidence.Protected;
                //hazardResource.AssignerId = hazard.AssignerId;
                hazardResource.AssignedOrganizationId = hazard.AssignedOrganizationId;
                //hazardResource.AssignedDepartmentId = hazard.AssignedDepartmentId;
                //hazardResource.ResolutionDate = hazard.ResolutionDate;

                // trigger alert when incidence status is open or re-opened
                if (hazardResource.IncidenceStatusId != hazard.IncidenceStatusId && (hazardResource.IncidenceStatusId == GlobalFields.OpenIncidenceStatus || hazardResource.IncidenceStatusId == GlobalFields.ReOpenedIncidenceStatus))
                {
                    hazardResource.AssignerId = _userRepo.GetLoggedInUserId();
                    triggerNotification = true;
                }
                else if (hazardResource.IncidenceStatusId != hazard.IncidenceStatusId && hazardResource.IncidenceStatusId == GlobalFields.ClosedIncidenceStatus)
                {
                    // set resolution date when incidence is closed
                    hazardResource.ResolutionDate = DateTime.Now;
                    triggerNotification = true;
                }
                else if (hazardResource.IncidenceStatusId != hazard.IncidenceStatusId && (hazardResource.IncidenceStatusId == GlobalFields.ResolvedIncidenceStatus || hazardResource.IncidenceStatusId == GlobalFields.UnderReviewIncidenceStatus))
                {
                    triggerNotification = true;
                }

                mapper.Map<SaveHazardResource, Hazard>(hazardResource, hazard);
            
                await unitOfWork.CompleteAsync();

                //send email and sms to notify assignee upon incidence opening and assignment
                //IncidenceType's mapped department's settings is used for sending notification, otherwise if empty, use organization or system settings 
                if (triggerNotification)
                    await _notificationRepo.SendIncidenceNotification(hazardResource, hazardResource.AssignedDepartmentId, hazardResource.AssignerId, hazardResource.AssigneeId, hazardResource.AssignedOrganizationId);

                hazard = await repository.GetHazard(id);
                var result = mapper.Map<Hazard, HazardResource>(hazard);
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
        [Route("createHazard")]
        public async Task<IActionResult> CreateHazard([FromBody] SaveHazardResource hazardResource)
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
                var hazardCode = !string.IsNullOrEmpty(hazardResource.Description) ? hazardResource.Description + "/"
                    + ((hazardResource.ReportedLatitude != null) ? hazardResource.ReportedLatitude : DateTime.Now.Minute) + "/"
                    + ((hazardResource.ReportedLongitude != null) ? hazardResource.ReportedLongitude : DateTime.Now.Minute)
                    : hazardResource.ReportedLatitude + "/" + hazardResource.ReportedLongitude;
                if (repository.CheckHazardCode(hazardCode))
                {
                    ModelState.AddModelError("Error", "The incidence with this Code already exists.");
                    return BadRequest(ModelState);
                }

                var hazard= mapper.Map<SaveHazardResource, Hazard>(hazardResource);
                hazard.Deleted = false;
                hazard.Protected = true;
                hazard.CreatedByUserId = _userRepo.GetLoggedInUserId();
                hazard.DateCreated = DateTime.Now;
                hazard.IncidenceStatusId = hazardResource.IncidenceStatusId ?? GlobalFields.NewIncidenceStatus;
                hazard.Code = hazardCode;

                if (user != null)
                {
                    var reporterFullName = string.IsNullOrEmpty(user.MiddleName) ? user.FirstName + " " + user.LastName
                        : user.FirstName + " " + user.MiddleName + " " + user.LastName;
                    hazard.ReporterName = reporterFullName ?? "";
                    hazard.ReporterDepartmentId = user.DepartmentId;
                    hazard.ReporterEmail = user.Email1;
                }

                hazard.OrganizationId = user.UserName != "Admin" ? user.OrganizationId : null;
                var organization = await _orgRepo.GetOrganization(hazard?.OrganizationId);
                hazard.AssignedOrganizationId = organization?.Id;
                hazard.AssignedDepartmentId = organization?.HazardDefaultDepartmentId;

                //if (organization.HazardDefaultDepartmentId != null)
                //    hazard.AssignedDepartmentId = organization.HazardDefaultDepartmentId;

                // trigger alert when incidence status is open or re-opened provided as Assignee and Assigner were sent
                if (hazardResource.AssigneeId != null && (hazardResource.IncidenceStatusId == GlobalFields.OpenIncidenceStatus || hazardResource.IncidenceStatusId == GlobalFields.ReOpenedIncidenceStatus))
                {
                    hazard.AssignerId = loggedInUserId;
                    triggerNotification = true;
                }
                else if (hazardResource.IncidenceStatusId != hazard.IncidenceStatusId && hazardResource.IncidenceStatusId == GlobalFields.ClosedIncidenceStatus)
                {
                    // set resolution date when incidence is closed. No notification here as it incidence was already closed before creation. No Assigner or Assignee before now
                    hazardResource.ResolutionDate = DateTime.Now;
                }

                hazard.IncidenceStatusId = hazardResource.IncidenceStatusId ?? GlobalFields.NewIncidenceStatus;
                if (hazard.IncidenceStatusId == GlobalFields.NewIncidenceStatus && hazard.AssignedDepartmentId != null)
                {
                    triggerDeptNotification = true;
                }

                //incidence.LastUpdate = DateTime.Now;
                repository.Add(hazard);
                await unitOfWork.CompleteAsync();

                if (triggerDeptNotification)
                {
                    //pass the reporter details into incidenceResource as it only exists in incidence and it is needed in email notification
                    hazardResource.ReporterName = hazard.ReporterName;
                    hazardResource.ReporterEmail = hazard.ReporterEmail;
                    hazard.ReporterDepartmentId = hazard.ReporterDepartmentId;
                    await _notificationRepo.SendHazardEmailNotificationToOrgDeptUsers(hazardResource, hazard.AssignedDepartmentId, hazard.AssignedOrganizationId, hazard.ReporterDepartmentId); // use SendEmailNotificationToOrgDeptUsers method in notificationRepo
                }

                hazard = await repository.GetHazard(hazard.Id);
                var result = mapper.Map<Hazard, HazardResource>(hazard);

                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }


        [HttpPost] //- To submit incidence from mobile app including single photo/video
        [Route("CreateHazardOnMobile")]
        public async Task<IActionResult> CreateHazardRemote([FromForm] SaveHazardResource hazardResource)
        {
            try
            {
                var loggedInUserId = _userRepo.GetLoggedInUserId();
                var user = await _userRepo.GetUser(loggedInUserId);
                bool triggerDeptNotification = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //business rule validation
                var hazardCode = !string.IsNullOrEmpty(hazardResource.Description) ? hazardResource.Description + "/"
                   + ((hazardResource.ReportedLatitude != null) ? hazardResource.ReportedLatitude : DateTime.Now.Minute) + "/"
                   + ((hazardResource.ReportedLongitude != null) ? hazardResource.ReportedLongitude : DateTime.Now.Minute)
                   : hazardResource.ReportedLatitude + "/" + hazardResource.ReportedLongitude;
                if (repository.CheckHazardCode(hazardCode))
                {
                    ModelState.AddModelError("Error", "The incidence with this Code already exists.");
                    return BadRequest(ModelState);
                }

                var hazard = mapper.Map<SaveHazardResource, Hazard>(hazardResource);
                hazard.Deleted = false;
                hazard.CreatedByUserId = loggedInUserId;
                // hazard.Protected = true;
                hazard.DateCreated = DateTime.UtcNow;
                hazard.Code = hazardCode;

                if (user != null)
                {
                    var reporterFullName = string.IsNullOrEmpty(user.MiddleName) ? user.FirstName + " " + user.LastName
                        : user.FirstName + " " + user.MiddleName + " " + user.LastName;
                    hazard.ReporterName = reporterFullName ?? "" ;
                    hazard.ReporterDepartmentId = user.DepartmentId;
                    hazard.ReporterEmail = user.Email1;
                }

                hazard.OrganizationId = user.UserName != "Admin" ? user.OrganizationId : null;
                var organization = await _orgRepo.GetOrganization(hazard?.OrganizationId);
                hazard.AssignedOrganizationId = organization?.Id;
                hazard.AssignedDepartmentId = organization?.HazardDefaultDepartmentId;

                hazard.IncidenceStatusId = hazardResource.IncidenceStatusId ?? GlobalFields.NewIncidenceStatus;
                if (hazard.IncidenceStatusId == GlobalFields.NewIncidenceStatus && hazard.AssignedDepartmentId != null)
                {
                    triggerDeptNotification = true;
                }

                repository.Add(hazard);
                await unitOfWork.CompleteAsync();

                hazard = await repository.GetHazard(hazard.Id);

                if (hazard == null)
                    return NotFound();

                if (hazardResource.file != null && hazardResource.file.Length != 0)
                {
                    var deptSettings = await _settingsRepo.GetDepartmentSettings(hazard.AssignedDepartmentId);
                    var orgSettings = await _settingsRepo.GetOrganizationSettings(hazard?.OrganizationId);
                    var generalSettings = await _settingsRepo.GetGeneralSettings();

                    if (!_settingsRepo.ValidateFileType(deptSettings, orgSettings, generalSettings, hazardResource.file))
                        return BadRequest("Invalid file type");

                    if (!_settingsRepo.ValidateFileSize(deptSettings, orgSettings, generalSettings, hazardResource.file))
                        return BadRequest("Maximun file size exceeded");

                    var isFilePhotoNotVideo = _settingsRepo.IsImageFileNotVideo(deptSettings, orgSettings, generalSettings, hazardResource.file);

                    var uploadsFolderPath = Path.Combine(_host.WebRootPath, "uploads");
                    var photo = await _photoService.UploadHazardPhoto(hazard, hazardResource.file, uploadsFolderPath, FileUploadChannels.incidencesReportedOnMobile, isFilePhotoNotVideo);

                    // return Ok(mapper.Map<Media, MediaResource>(photo));
                }

                //Next, fetch notification settings from the IncidenceType's-mapped department or the mapped department's organization if dept settings is empty. Dept receives notification here
                //IncidenceType's mapped department's settings is used for sending notification, otherwise if empty, use organization or system settings  
                await _notificationRepo.SendIncidenceNotification(hazardResource, hazard.AssignedDepartmentId, null, null, hazard?.OrganizationId);

                if (triggerDeptNotification)
                {
                    //pass the reporter details into incidenceResource as it only exists in incidence and it is needed in email notification
                    hazardResource.ReporterName = hazard.ReporterName;
                    hazardResource.ReporterEmail = hazard.ReporterEmail;
                    hazard.ReporterDepartmentId = hazard.ReporterDepartmentId;
                    await _notificationRepo.SendHazardEmailNotificationToOrgDeptUsers(hazardResource, hazard.AssignedDepartmentId, hazard.AssignedOrganizationId, hazard.ReporterDepartmentId); // use SendEmailNotificationToOrgDeptUsers method in notificationRepo
                }

                hazard = await repository.GetHazard(hazard.Id);
                var result = mapper.Map<Hazard, HazardResource>(hazard);

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
        public async Task<IActionResult> DeleteHazard([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var hazard = await repository.GetHazard(id, includeRelated: false);

                if (hazard == null )
                {
                    return NotFound();
                }

                if (hazard.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be deleted");
                    return BadRequest(ModelState);
                }

                hazard.Deleted = true;
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
        [Route("sendHazardReporterRating/{id}/{rating}")]
        public async Task<IActionResult> SendHazardReporterRating([FromRoute] Guid id, [FromRoute] int rating)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var hazard = await repository.GetHazard(id, includeRelated: false);

                if (hazard == null)
                {
                    return NotFound();
                }

                hazard.ReporterFeedbackRating = rating;

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