using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Helpers.Abstract
{
    public interface IPhotoService
    {
        Task<Media> UploadPhoto(Incidence incidence, IFormFile file, string uploadsFolderPath, FileUploadChannels channelId, bool IsImageFileNotVideo = false);
        Task<Media> UploadHazardPhoto(Hazard hazard, IFormFile file, string uploadsFolderPath, FileUploadChannels channelId, bool IsImageFileNotVideo = false);
        Task<Media> UploadUserProfilePhoto(User user, IFormFile file, string uploadsFolderPath, bool IsImageFileNotVideo = false);
    }
}
