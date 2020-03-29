using IRS.API.Dtos;
using IRS.API.Helpers.Abstract;
using IRS.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IRS.API.Helpers
{
    public class SettingsRepository : ISettingsRepository
    {
        //repository: collection of domain objects in memory
        private readonly ApplicationDbContext context;

        public SettingsRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<NotificationSettings> GetOrganizationSettings(Guid? organizationId)
        {
            if (organizationId != null)
            {
                var query = (from org in context.Organizations
                             select new
                             {
                                 Id = org.Id,
                                 Email1 = org.Email1,
                                 Email2 = org.Email2,
                                 UseSsl = org.UseSsl,
                                 HostName = org.HostName,
                                 Port = org.Port,
                                 MaxImageFileSize = org.MaxImageFileSize,
                                 MaxVideoFileSize = org.MaxVideoFileSize,
                                 AcceptedImageFileTypes = org.AcceptedImageFileTypes,
                                 AcceptedVideoFileTypes = org.AcceptedVideoFileTypes,
                                 PageSize = org.PageSize,
                                 SmsServiceProvider = org.SmsServiceProvider,
                                 EmailSendersEmail = org.EmailSendersEmail,
                                 EmailSenderName = org.EmailSenderName,
                                 EmailSenderPassword = org.EmailSenderPassword,
                                 SendSMS = org.SendSMS,
                                 EntityName = org.CompanyName,
                                 EmailRecipientAddresses = org.Email1, //receive emails on single email in general settings
                                 SMSRecipientNumbers = org.Phone1,
                                 AssignedOrganizationName = org.CompanyName,
                                 AssignedDepartmentName = "",
                                 ActivateEmailSenderSettings = org.ActivateEmailSenderSettings,
                                 ActivateFileSettings = org.ActivateFileSettings
                             }).ToList().Select((s) => new NotificationSettings()
                             {
                                 Id = s.Id,
                                 Email1 = s.Email1,
                                 Email2 = s.Email2,
                                 UseSsl = s.UseSsl,
                                 HostName = s.HostName,
                                 Port = s.Port,
                                 MaxImageFileSize = s.MaxImageFileSize * 1024 * 1024, //mb to bytes
                                 MaxVideoFileSize = s.MaxVideoFileSize * 1024 * 1024, //mb to bytes
                                 AcceptedImageFileTypes = string.IsNullOrEmpty(s.AcceptedImageFileTypes) ? null : s.AcceptedImageFileTypes.Split(',', StringSplitOptions.None).Select(x => x.ToString()).ToList(),
                                 AcceptedVideoFileTypes = string.IsNullOrEmpty(s.AcceptedVideoFileTypes) ? null : s.AcceptedVideoFileTypes.Split(',', StringSplitOptions.None).Select(x => x.ToString()).ToList(),
                                 AcceptedVideoFileTypesString = s.AcceptedVideoFileTypes,
                                 AcceptedImageFileTypesString = s.AcceptedImageFileTypes,
                                 PageSize = s.PageSize,
                                 SmsServiceProvider = s.SmsServiceProvider,
                                 EmailSendersEmail = s.EmailSendersEmail,
                                 EmailSenderName = s.EmailSenderName,
                                 EmailSenderPassword = s.EmailSenderPassword,
                                 SendSMS = s.SendSMS,
                                 EntityName = s.EntityName,
                                 EmailRecipientAddresses = string.IsNullOrEmpty(s.EmailRecipientAddresses) ? null : s.EmailRecipientAddresses.Split(',', StringSplitOptions.None).Select(x => x.ToString()).ToList(),
                                 EmailRecipientAddressesString = s.EmailRecipientAddresses,
                                 SMSRecipientNumbers = string.IsNullOrEmpty(s.SMSRecipientNumbers) ? null : s.SMSRecipientNumbers.Split(',', StringSplitOptions.None).Select(x => x.ToString()).ToList(),
                                 SMSRecipientNumbersString = s.SMSRecipientNumbers,
                                 AssignedOrganizationName = s.AssignedOrganizationName,
                                 AssignedDepartmentName = s.AssignedDepartmentName,
                                 ActivateEmailSenderSettings = s.ActivateEmailSenderSettings,
                                 ActivateFileSettings = s.ActivateFileSettings
                             }).FirstOrDefault(x => x.Id == organizationId);

                return query;
            }

            return null;
        }

        public async Task<NotificationSettings> GetGeneralSettings()
        {
            var query = (from org in context.SystemSettings
                         select new
                         {
                             Id = org.Id,
                             Email1 = org.Email1,
                             Email2 = org.Email2,
                             UseSsl = org.UseSsl,
                             HostName = org.HostName,
                             Port = org.Port,
                             MaxImageFileSize = org.MaxImageFileSize,
                             AcceptedImageFileTypes = org.AcceptedImageFileTypes,
                             MaxVideoFileSize = org.MaxVideoFileSize,
                             AcceptedVideoFileTypes = org.AcceptedVideoFileTypes,
                             PageSize = org.PageSize,
                             SmsServiceProvider = org.SmsServiceProvider,
                             EmailSendersEmail = org.EmailSendersEmail,
                             EmailSenderName = org.EmailSenderName,
                             EmailSenderPassword = org.EmailSenderPassword,
                             SendSMS = org.SendSMS,
                             EntityName = org.EmailSenderName,
                             EmailRecipientAddresses = org.Email1, //receive emails on single email in general settings
                             SMSRecipientNumbers = org.Phone1,
                             AssignedOrganizationName = "",
                             AssignedDepartmentName = ""
                         }).ToList().Select((s) => new NotificationSettings()
                         {
                             Id = s.Id,
                             Email1 = s.Email1,
                             Email2 = s.Email2,
                             UseSsl = s.UseSsl,
                             HostName = s.HostName,
                             Port = s.Port,
                             MaxImageFileSize = s.MaxImageFileSize * 1024 * 1024, //mb to bytes
                             MaxVideoFileSize = s.MaxVideoFileSize * 1024 * 1024, //mb to bytes
                             AcceptedImageFileTypes = string.IsNullOrEmpty(s.AcceptedImageFileTypes) ? null : s.AcceptedImageFileTypes.Split(',', StringSplitOptions.None).Select(x => x.ToString()).ToList(),
                             AcceptedVideoFileTypes = string.IsNullOrEmpty(s.AcceptedVideoFileTypes) ? null : s.AcceptedVideoFileTypes.Split(',', StringSplitOptions.None).Select(x => x.ToString()).ToList(),
                             AcceptedVideoFileTypesString = s.AcceptedVideoFileTypes,
                             AcceptedImageFileTypesString = s.AcceptedImageFileTypes,
                             PageSize = s.PageSize,
                             SmsServiceProvider = s.SmsServiceProvider,
                             EmailSendersEmail = s.EmailSendersEmail,
                             EmailSenderName = s.EmailSenderName,
                             EmailSenderPassword = s.EmailSenderPassword,
                             SendSMS = s.SendSMS,
                             EntityName = s.EntityName,
                             EmailRecipientAddresses = string.IsNullOrEmpty(s.EmailRecipientAddresses) ? null : s.EmailRecipientAddresses.Split(',', StringSplitOptions.None).Select(x => x.ToString()).ToList(),
                             EmailRecipientAddressesString = s.EmailRecipientAddresses,
                             SMSRecipientNumbers = string.IsNullOrEmpty(s.SMSRecipientNumbers) ? null : s.SMSRecipientNumbers.Split(',', StringSplitOptions.None).Select(x => x.ToString()).ToList(),
                             SMSRecipientNumbersString = s.SMSRecipientNumbers,
                             AssignedOrganizationName = s.AssignedOrganizationName,
                             AssignedDepartmentName = s.AssignedDepartmentName
                         }).FirstOrDefault();

            return query;

        }



        public async Task<NotificationSettings> GetDepartmentSettings(Guid? deptId)
        {
            var query = (from dept in context.OrganizationDepartments
                         select new
                         {
                             Id = dept.Id,
                             Email1 = dept.Email1,
                             Email2 = dept.Email2,
                             UseSsl = dept.UseSsl,
                             HostName = dept.HostName,
                             Port = dept.Port,
                             MaxImageFileSize = dept.MaxImageFileSize,
                             AcceptedImageFileTypes = dept.AcceptedImageFileTypes,
                             MaxVideoFileSize = dept.MaxVideoFileSize,
                             AcceptedVideoFileTypes = dept.AcceptedVideoFileTypes,
                             PageSize = dept.PageSize,
                             SmsServiceProvider = dept.SmsServiceProvider,
                             EmailSendersEmail = dept.EmailSendersEmail,
                             EmailSenderName = dept.EmailSenderName,
                             EmailSenderPassword = dept.EmailSenderPassword,
                             SendSMS = dept.SendSMS,
                             EntityName = dept.Name,
                             EmailRecipientAddresses = dept.EmailRecipientAddresses,
                             SMSRecipientNumbers = dept.SMSRecipientNumbers,
                             AssignedOrganizationName = "",
                             AssignedDepartmentName = dept.Name,
                             ActivateEmailSenderSettings = dept.ActivateEmailSenderSettings,
                             ActivateFileSettings = dept.ActivateFileSettings
                         }).ToList().Select((s) => new NotificationSettings()
                         {
                             Id = s.Id,
                             Email1 = s.Email1,
                             Email2 = s.Email2,
                             UseSsl = s.UseSsl,
                             HostName = s.HostName,
                             Port = s.Port,
                             MaxImageFileSize = s.MaxImageFileSize * 1024 * 1024, //mb to bytes
                             MaxVideoFileSize = s.MaxVideoFileSize * 1024 * 1024,
                             AcceptedImageFileTypes = string.IsNullOrEmpty(s.AcceptedImageFileTypes) ? null : s.AcceptedImageFileTypes.Split(',', StringSplitOptions.None).Select(x => x.ToString()).ToList(),
                             AcceptedVideoFileTypes = string.IsNullOrEmpty(s.AcceptedVideoFileTypes) ? null : s.AcceptedVideoFileTypes.Split(',', StringSplitOptions.None).Select(x => x.ToString()).ToList(),
                             AcceptedVideoFileTypesString = s.AcceptedVideoFileTypes,
                             AcceptedImageFileTypesString = s.AcceptedImageFileTypes,
                             PageSize = s.PageSize,
                             SmsServiceProvider = s.SmsServiceProvider,
                             EmailSendersEmail = s.EmailSendersEmail,
                             EmailSenderName = s.EmailSenderName,
                             EmailSenderPassword = s.EmailSenderPassword,
                             SendSMS = s.SendSMS,
                             EntityName = s.EntityName,
                             EmailRecipientAddresses = string.IsNullOrEmpty(s.EmailRecipientAddresses) ? null : s.EmailRecipientAddresses.Split(',', StringSplitOptions.None).Select(x => x.ToString()).ToList(),
                             EmailRecipientAddressesString = s.EmailRecipientAddresses,
                             SMSRecipientNumbers = string.IsNullOrEmpty(s.SMSRecipientNumbers) ? null : s.SMSRecipientNumbers.Split(',', StringSplitOptions.None).Select(x => x.ToString()).ToList(),
                             SMSRecipientNumbersString = s.SMSRecipientNumbers,
                             AssignedOrganizationName = s.AssignedOrganizationName,
                             AssignedDepartmentName = s.AssignedDepartmentName,
                             ActivateEmailSenderSettings = s.ActivateEmailSenderSettings,
                             ActivateFileSettings = s.ActivateFileSettings
                         }).FirstOrDefault(x => x.Id == deptId);

            return query;

        }

        public bool ValidateFileSize(NotificationSettings deptSettings, NotificationSettings orgSettings, NotificationSettings generalSettings, IFormFile file)
        {
            NotificationSettings activeSettings;
            float? MaxFileSize = 50 * 1024;
            if (deptSettings != null && deptSettings.ActivateFileSettings == true && deptSettings != null && deptSettings.MaxImageFileSize != null)
                activeSettings = deptSettings;
            else if (orgSettings != null && orgSettings.ActivateFileSettings == true && orgSettings != null && orgSettings.MaxImageFileSize != null)
                activeSettings = orgSettings;
            else activeSettings = generalSettings;
            if (activeSettings != null)
            {
                if(activeSettings.IsSupportedImageFile(file.FileName))
                {
                    if (activeSettings.MaxImageFileSize != null)
                        MaxFileSize = activeSettings.MaxImageFileSize;
                }
                else if (activeSettings.IsSupportedVideoFile(file.FileName))
                {
                    if (activeSettings.MaxVideoFileSize != null)
                        MaxFileSize = activeSettings.MaxVideoFileSize;
                }
            }

            if (file.Length < MaxFileSize) return true;

            return false;
        }

        public bool ValidateFileType(NotificationSettings deptSettings, NotificationSettings orgSettings, NotificationSettings generalSettings, IFormFile file)
        {
            NotificationSettings activeSettings;
            if (deptSettings != null && deptSettings.ActivateFileSettings == true && deptSettings.AcceptedImageFileTypes != null && deptSettings.AcceptedVideoFileTypes != null)
                activeSettings = deptSettings;
            else if (orgSettings != null && orgSettings.ActivateFileSettings == true && orgSettings.AcceptedImageFileTypes != null && orgSettings.AcceptedVideoFileTypes != null)
                activeSettings = orgSettings;
            else activeSettings = generalSettings;
            if (activeSettings != null)
            {
                if (activeSettings.AcceptedImageFileTypes != null)
                {
                    if (activeSettings.IsSupportedImageFile(file.FileName)) return true;
                }
                if (activeSettings.AcceptedVideoFileTypes != null)
                {
                    if (activeSettings.IsSupportedVideoFile(file.FileName)) return true;
                }
            }

            return false;
        }

        public bool IsImageFileNotVideo(NotificationSettings deptSettings, NotificationSettings orgSettings, NotificationSettings generalSettings, IFormFile file)
        {
            NotificationSettings activeSettings;
            if (deptSettings != null && deptSettings.ActivateFileSettings == true && deptSettings.AcceptedImageFileTypes != null && deptSettings.AcceptedVideoFileTypes != null)
                activeSettings = deptSettings;
            else if (orgSettings != null && orgSettings.ActivateFileSettings == true && orgSettings.AcceptedImageFileTypes != null && orgSettings.AcceptedVideoFileTypes != null)
                activeSettings = orgSettings;
            else activeSettings = generalSettings;
            if (activeSettings != null)
            {
                if (activeSettings.AcceptedImageFileTypes != null)
                {
                    if (activeSettings.IsSupportedImageFile(file.FileName)) return true;
                }
                if (activeSettings.AcceptedVideoFileTypes != null)
                {
                    if (activeSettings.IsSupportedVideoFile(file.FileName)) return false;
                }
            }

            return false;
        }
    }
}
