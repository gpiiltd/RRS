using IRS.DAL.ModelInterfaces;
using IRS.DAL.Models.Identity;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRS.DAL.Models
{

    public enum SMSServiceProvider
    {
        Twilio
    }

    public class Organization: BaseModel, IEditLoggable, ICreateLoggable, IPseudoDeletable, IProtectable
    {
        public Organization()
        {
            AllocatedIncidences = new HashSet<Incidence>();
            OrganizationIncidences = new HashSet<Incidence>();
            OrganizationDepartments = new HashSet<OrganizationDepartment>();
            Users = new HashSet<User>();
        }
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
        [Display(Name = "Date created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Created by")]
        public Guid CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public virtual User CreatedByUser { get; set; }

        [Display(Name = "Edited on")]
        public DateTime? DateEdited { get; set; }

        [Display(Name = "Edited by")]
        public Guid? EditedByUserId { get; set; }

        [ForeignKey(nameof(EditedByUserId))]
        public virtual User EditedByUser { get; set; }

        //location
        [ForeignKey("AreaId")]
        [Display(Name = "Area")]
        public virtual Area Area { get; set; }

        [Display(Name = "Area")]
        public Guid? AreaId { get; set; }

        [ForeignKey("CityId")]
        [Display(Name = "City")]
        public virtual City City { get; set; }

        [Display(Name = "City")]
        public Guid? CityId { get; set; }

        [ForeignKey("StateId")]
        [Display(Name = "State")]
        public virtual State State { get; set; }

        [Display(Name = "State")]
        public Guid? StateId { get; set; }

        [ForeignKey("CountryId")]
        [Display(Name = "Country")]
        public virtual Country Country { get; set; }

        [Display(Name = "Country")]
        public Guid? CountryId { get; set; }

        public bool EnableBranding { get; set; }
        public string BrandCssStyle { get; set; }
        public string PreferredLanguage { get; set; }
        public string BrandTitle { get; set; }
        //public string BrandLogo { get; set; }
        public string BrandIcon { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public bool UseSsl { get; set; }
        public string HostName { get; set; }
        public int? Port { get; set; }
        public float? MaxImageFileSize { get; set; }
        public string AcceptedImageFileTypes { get; set; }
        public float? MaxVideoFileSize { get; set; }
        public string AcceptedVideoFileTypes { get; set; }
        public int? PageSize { get; set; }
        public bool Protected { get; set; }
        public bool Deleted { get; set; }
        public ICollection<Incidence> AllocatedIncidences { get; set; }

        public ICollection<Incidence> OrganizationIncidences { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<OrganizationDepartment> OrganizationDepartments { get; set; }
        public SMSServiceProvider? SmsServiceProvider { get; set; }
        public string EmailSenderName { get; set; }

        public string EmailSendersEmail { get; set; }

        public string EmailSenderPassword { get; set; }
        public string EmailRecipientAddresses { get; set; }
        public bool SendSMS { get; set; }
        public string SMSRecipientNumbers { get; set; }
        public string SMSSenderName { get; set; }
        public bool ActivateEmailSenderSettings { get; set; }
        public bool ActivateFileSettings { get; set; }

        [Display(Name = "Hazard Default Department")]
        public Guid? HazardDefaultDepartmentId { get; set; }
    }
}
