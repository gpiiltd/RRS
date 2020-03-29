using IRS.DAL.Models.Identity;
using IRS.DAL.Models.Shared;
using IRS.DAL.Models.SharedResource;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.Models.OrganizationAndDepartments
{
    public class OrganizationDto
    {
        public Guid? Id { get; set; }
        public int? SerialNumber { get; set; }
        public string CompanyName { get; set; }
        public string RegistrationNo { get; set; }
        public string BusinessCategory { get; set; }
        public string Code { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactMiddleName { get; set; }
        public string ContactLastName { get; set; }

        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string OfficeAddress { get; set; }
        public string BrandLogo { get; set; }

        public DateTime? DateofEst { get; set; }
        public string Comment { get; set; }

        //audit
        public DateTime? DateCreated { get; set; }

        public Guid? CreatedByUserId { get; set; }

        public DateTime? DateEdited { get; set; }
        public virtual User CreatedByUser { get; set; }

        public Guid? EditedByUserId { get; set; }
        //public virtual KeyValuePair EditedByUser { get; set; }

        //location
        //public virtual Area Area { get; set; }

        public virtual KeyValuePairResource Area { get; set; }
        public Guid? AreaId { get; set; }

        public virtual KeyValuePairResource City { get; set; }
        public Guid? CityId { get; set; }

        public virtual KeyValuePairResource State { get; set; }
        public Guid? StateId { get; set; }

        public virtual KeyValuePairResource Country { get; set; }
        public Guid? CountryId { get; set; }

        public bool? EnableBranding { get; set; }
        public string BrandCssStyle { get; set; }
        public string PreferredLanguage { get; set; }
        public string BrandTitle { get; set; }
        //public string BrandLogo { get; set; }
        public string BrandIcon { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public bool? UseSsl { get; set; }
        public string HostName { get; set; }
        public string Port { get; set; }
        public float? MaxImageFileSize { get; set; }
        public string AcceptedImageFileTypes { get; set; }
        public float? MaxVideoFileSize { get; set; }
        public string AcceptedVideoFileTypes { get; set; }
        public int? PageSize { get; set; }
        public bool? Deleted { get; set; }
        public bool? Protected { get; set; }
        public SMSServiceProvider? SmsServiceProvider { get; set; }
        public string EmailSenderName { get; set; }

        public string EmailSendersEmail { get; set; }

        public string EmailSenderPassword { get; set; }
        public string EmailRecipientAddresses { get; set; }
        public bool? SendSMS { get; set; }
        public string SMSRecipientNumbers { get; set; }
        public string SMSSenderName { get; set; }
        public bool? ActivateEmailSenderSettings { get; set; }
        public bool? ActivateFileSettings { get; set; }
        public Guid? HazardDefaultDepartmentId { get; set; }
    }
}
