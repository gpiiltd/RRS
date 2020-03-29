using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using IRS.DAL.Models;

namespace IRS.API.Helpers
{
    public class NotificationSettings
    {
        public Guid? Id { get; set; }
        public string EntityName { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public bool? UseSsl { get; set; }
        public string HostName { get; set; }
        public int? Port { get; set; }
        public int? PageSize { get; set; }
        public float? MaxImageFileSize { get; set; }
        public List<string> AcceptedImageFileTypes { get; set; }

        
        public float? MaxVideoFileSize { get; set; }
        public List<string> AcceptedVideoFileTypes { get; set; }
        public string AcceptedVideoFileTypesString { get; set; }
        public string AcceptedImageFileTypesString { get; set; }

        public SMSServiceProvider? SmsServiceProvider { get; set; }
        public string EmailSenderName { get; set; }

        public string EmailSendersEmail { get; set; }
        public string OrganizationName { get; set; }
        public string AssignedOrganizationName { get; set; }
        public string AssignedDepartmentName { get; set; }

        public string EmailSenderPassword { get; set; }

        public bool SendSMS { get; set; }
        public bool ActivateEmailSenderSettings { get; set; }
        public bool ActivateFileSettings { get; set; }

        public List<string> EmailRecipientAddresses { get; set; }
        public string EmailRecipientAddressesString { get; set; }
        public string SMSRecipientNumbersString { get; set; }
        

        public List<string> SMSRecipientNumbers { get; set; }
        public string SMSSenderName { get; set; }

        public bool IsSupportedImageFile(string fileName)
        {
            if (AcceptedImageFileTypes != null)
                return AcceptedImageFileTypes.Any(s => s == Path.GetExtension(fileName).ToLower());

            return false;
        }

        public bool IsSupportedVideoFile(string fileName)
        {
            if (AcceptedVideoFileTypes != null)
                return AcceptedVideoFileTypes.Any(s => s == Path.GetExtension(fileName).ToLower());

            return false;
        }
    }
}
