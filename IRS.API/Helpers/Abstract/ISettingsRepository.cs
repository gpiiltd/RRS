using IRS.API.Dtos;
using IRS.API.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Helpers.Abstract
{
    public interface ISettingsRepository
    {
        Task<NotificationSettings> GetOrganizationSettings(Guid? organizationId);
        Task<NotificationSettings> GetGeneralSettings();
        Task<NotificationSettings> GetDepartmentSettings(Guid? deptId);
        bool ValidateFileSize(NotificationSettings deptSettings, NotificationSettings orgSettings, NotificationSettings generalSettings, IFormFile file);
        bool ValidateFileType(NotificationSettings deptSettings, NotificationSettings orgSettings, NotificationSettings generalSettings, IFormFile file);
        bool IsImageFileNotVideo(NotificationSettings deptSettings, NotificationSettings orgSettings, NotificationSettings generalSettings, IFormFile file);
    }
}
