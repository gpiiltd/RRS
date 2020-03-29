using IRS.API.Helpers.Abstract;
using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Helpers
{
    public class PhotoService: IPhotoService
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IPhotoStorage _photoStorage;
        public PhotoService(IUnitofWork unitOfWork, IPhotoStorage photoStorage)
        {
            this._photoStorage = photoStorage;
            this._unitOfWork = unitOfWork;
        }

        public async Task<Media> UploadPhoto(Incidence incidence, IFormFile file, string uploadsFolderPath, FileUploadChannels channelId = FileUploadChannels.incidencesResolvedOnWeb, bool IsImageFileNotVideo = false )
        {
            var fileName = await _photoStorage.StorePhoto(uploadsFolderPath, file);

            var media = new Media { FileName = fileName, DateUploaded = DateTime.Now, FileUploadChannel = channelId, Description = file.FileName, IsVideo = !IsImageFileNotVideo };
            incidence.Medias.Add(media);
            await _unitOfWork.CompleteAsync();

            return media;
        }

        public async Task<Media> UploadHazardPhoto(Hazard hazard, IFormFile file, string uploadsFolderPath, FileUploadChannels channelId = FileUploadChannels.incidencesResolvedOnWeb, bool IsImageFileNotVideo = false)
        {
            var fileName = await _photoStorage.StorePhoto(uploadsFolderPath, file);

            var media = new Media { FileName = fileName, DateUploaded = DateTime.Now, FileUploadChannel = channelId, Description = file.FileName, IsVideo = !IsImageFileNotVideo };
            hazard.Medias.Add(media);
            await _unitOfWork.CompleteAsync();

            return media;
        }

        public async Task<Media> UploadUserProfilePhoto(User user, IFormFile file, string uploadsFolderPath, bool IsImageFileNotVideo = false)
        {
            var fileName = await _photoStorage.StorePhoto(uploadsFolderPath, file);

            var media = new Media { FileName = fileName, DateUploaded = DateTime.Now, Description = file.FileName, IsVideo = !IsImageFileNotVideo };
            user.Medias.Add(media);
            await _unitOfWork.CompleteAsync();

            return media;
        }
    }
}
