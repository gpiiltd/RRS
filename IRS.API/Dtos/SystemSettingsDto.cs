using IRS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Dtos
{
    public class SystemSettingsDto
    {
        public Guid? Id { get; set; }
        public string BrandCssStyle { get; set; }
        public string PreferredLanguage { get; set; }
        public string BrandTitle { get; set; }
        public string BrandLogo { get; set; }
        public string BrandIcon { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public bool UseSsl { get; set; }
        public string HostName { get; set; }
        public int? Port { get; set; }
        public float? MaxImageFileSize { get; set; }
        public string AcceptedImageFileTypes { get; set; }
        //public string AcceptedImageFileTypesString { get; set; }

        public float? MaxVideoFileSize { get; set; }
        public string AcceptedVideoFileTypes { get; set; }
        //public string AcceptedVideoFileTypesString { get; set; }

        public int? PageSize { get; set; }
        public SMSServiceProvider? SmsServiceProvider { get; set; }
        public string EmailSenderName { get; set; }

        public string OrganizationName { get; set; }

        public string EmailSendersEmail { get; set; }
        public string EmailSenderPassword { get; set; }
        public string EmailRecipientAddresses { get; set; }
        public bool SendSMS { get; set; }
        public string SMSRecipientNumbers { get; set; }
        public string SMSSenderName { get; set; }
    }
}
