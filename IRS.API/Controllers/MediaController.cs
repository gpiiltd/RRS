using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IRS.API.Dtos;
using IRS.API.Helpers;
using IRS.API.Helpers.Abstract;
using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IRS.API.Controllers
{
    [RequestSizeLimit(1048576000)]
    [Produces("application/json")]
    public class MediaController : ControllerBase
    {
        private readonly IHostingEnvironment _host;
        private readonly IUnitofWork _unitOfWork;
        private readonly IIncidenceRepository _incidenceRepo;
        private readonly IHazardRepository _hazardRepo;
        private readonly IMediaRepository _photoRepo;
        private readonly IPhotoService _photoService;
        private readonly NotificationSettings _notificationSettings;
        private readonly IMapper _mapper;
        private readonly ISettingsRepository _settingsRepo; 
        private readonly IUserRepository _userRepo;

        public MediaController(IHostingEnvironment host, IUnitofWork unitOfWork, IIncidenceRepository incidenceRepo, IMapper mapper,
            IMediaRepository photoRepo, IPhotoService photoService, ISettingsRepository settingsRepo, IUserRepository userRepo, IHazardRepository hazardRepo)
        {
            _host = host;
            _unitOfWork = unitOfWork;
            _incidenceRepo = incidenceRepo;
            _photoRepo = photoRepo;
            _mapper = mapper;
            _photoService = photoService;
            _settingsRepo = settingsRepo;
            _userRepo = userRepo;
            _hazardRepo = hazardRepo;
        }

        [HttpGet]
        [Route("api/incidence/{incidenceId}/Media")]
        public async Task<IActionResult> GetPhotos(Guid incidenceId)
        {
            var photosFromRepo = await _photoRepo.GetPhotos(incidenceId);
            var photoListDto = _mapper.Map<IEnumerable<MediaResource>>(photosFromRepo);
            return Ok(photoListDto);
        }

        [HttpGet]
        [Route("api/hazard/{hazardId}/Media")]
        public async Task<IActionResult> GetHazardPhotos(Guid hazardId)
        {
            var photosFromRepo = await _photoRepo.GetHazardPhotos(hazardId);
            var photoListDto = _mapper.Map<IEnumerable<MediaResource>>(photosFromRepo);
            return Ok(photoListDto);
        }

        [HttpGet]
        [Route("api/users/{userId}/Media")]
        public async Task<IActionResult> GetUserProfilePhoto(Guid userId)
        {
            var photosFromRepo = await _photoRepo.GetUserProfilePhoto(userId);
            var photoListDto = _mapper.Map<MediaResource>(photosFromRepo);
            return Ok(photoListDto);
        }

        [HttpPost]
        [Route("api/incidence/{incidenceId}/Media/{channelId}")]
        public async Task<IActionResult> UploadIncidencePhotosVideos(Guid incidenceId, IFormFile file, FileUploadChannels channelId)
        {
            try
            {
                // upload photo/video for incidence
                var incidence = await _incidenceRepo.GetIncidence(incidenceId, includeRelated: false);
                if (incidence == null)
                    return NotFound();

                if (file == null) return BadRequest("Null file");
                if (file.Length == 0) return BadRequest("Empty file");

                var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
                var userdeployment = await _userRepo.GetUserDeploymentData(_userRepo.GetLoggedInUserId());

                var orgSettings = await _settingsRepo.GetOrganizationSettings(user.OrganizationId);
                var deptSettings = await _settingsRepo.GetDepartmentSettings(userdeployment.DepartmentId);
                var generalSettings = await _settingsRepo.GetGeneralSettings();


                //uploading incidence resolution proof uses the uploader's dept settings|org settings|general settings
                if (!_settingsRepo.ValidateFileType(deptSettings, orgSettings, generalSettings, file))
                    return BadRequest("Invalid file type");

                if (!_settingsRepo.ValidateFileSize(deptSettings, orgSettings, generalSettings, file))
                    return BadRequest("Maximun file size exceeded");

                var isFilePhotoNotVideo = _settingsRepo.IsImageFileNotVideo(deptSettings, orgSettings, generalSettings, file);

                var uploadsFolderPath = Path.Combine(_host.WebRootPath, "uploads");
                var photo = await _photoService.UploadPhoto(incidence, file, uploadsFolderPath, channelId, isFilePhotoNotVideo);

                return Ok(_mapper.Map<Media, MediaResource>(photo));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("api/hazard/{hazardId}/Media/{channelId}")]
        public async Task<IActionResult> UploadHazardPhotosVideos(Guid hazardId, IFormFile file, FileUploadChannels channelId)
        {
            try
            {
                // upload photo/video for incidence
                var hazard = await _hazardRepo.GetHazard(hazardId, includeRelated: false);
                if (hazard == null)
                    return NotFound();

                if (file == null) return BadRequest("Null file");
                if (file.Length == 0) return BadRequest("Empty file");

                var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
                var userdeployment = await _userRepo.GetUserDeploymentData(_userRepo.GetLoggedInUserId());

                var orgSettings = await _settingsRepo.GetOrganizationSettings(user.OrganizationId);
                var deptSettings = await _settingsRepo.GetDepartmentSettings(userdeployment.DepartmentId);
                var generalSettings = await _settingsRepo.GetGeneralSettings();


                //uploading incidence resolution proof uses the uploader's dept settings|org settings|general settings
                if (!_settingsRepo.ValidateFileType(deptSettings, orgSettings, generalSettings, file))
                    return BadRequest("Invalid file type");

                if (!_settingsRepo.ValidateFileSize(deptSettings, orgSettings, generalSettings, file))
                    return BadRequest("Maximun file size exceeded");

                var isFilePhotoNotVideo = _settingsRepo.IsImageFileNotVideo(deptSettings, orgSettings, generalSettings, file);

                var uploadsFolderPath = Path.Combine(_host.WebRootPath, "uploads");
                var photo = await _photoService.UploadHazardPhoto(hazard, file, uploadsFolderPath, channelId, isFilePhotoNotVideo);

                return Ok(_mapper.Map<Media, MediaResource>(photo));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("api/users/{userId}/Media/")]
        public async Task<IActionResult> UploadProfilePhoto(Guid userId, IFormFile file)
        {
            try
            {
                // upload user profile photo
                var user = await _userRepo.GetUser(userId);
                if (user == null)
                    return NotFound();

                if (file == null) return BadRequest("Null file");
                if (file.Length == 0) return BadRequest("Empty file");

                var loggedinUser = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
                var userdeployment = await _userRepo.GetUserDeploymentData(_userRepo.GetLoggedInUserId());

                var orgSettings = await _settingsRepo.GetOrganizationSettings(loggedinUser.OrganizationId);
                var deptSettings = await _settingsRepo.GetDepartmentSettings(userdeployment.DepartmentId);
                var generalSettings = await _settingsRepo.GetGeneralSettings();


                //uploading incidence resolution proof uses the uploader's dept settings|org settings|general settings
                var isFilePhotoNotVideo = _settingsRepo.IsImageFileNotVideo(deptSettings, orgSettings, generalSettings, file);
                if (!isFilePhotoNotVideo)
                    return BadRequest("Invalid file type");

                if (!_settingsRepo.ValidateFileSize(deptSettings, orgSettings, generalSettings, file))
                    return BadRequest("Maximun file size exceeded");

                //check if user already has a profile photo and delete, replace with new one
                var userProfilePhotoData = await _photoRepo.GetUserProfilePhoto(userId);
                if (userProfilePhotoData != null)
                {
                    _photoRepo.Delete(userProfilePhotoData);
                    await _unitOfWork.CompleteAsync();
                }

                var uploadsFolderPath = Path.Combine(_host.WebRootPath, "uploads/userProfileData");
                var photo = await _photoService.UploadUserProfilePhoto(user, file, uploadsFolderPath, isFilePhotoNotVideo);

                return Ok(_mapper.Map<Media, MediaResource>(photo));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("api/deletePhotoVideo/{Id}")]
        public async Task<IActionResult> DeleteIncidencePhoto([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var iPhotoData = await _photoRepo.GetPhoto(id);

                if (iPhotoData == null)
                {
                    return NotFound();
                }

                _photoRepo.Delete(iPhotoData);
                await _unitOfWork.CompleteAsync();

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