using IRS.DAL.ModelInterfaces;
using IRS.DAL.Models.Identity;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IRS.DAL.Models
{
    public class OrganizationDepartment: BaseModel, IPseudoDeletable, IProtectable
    {
        public OrganizationDepartment()
        {
            Users = new HashSet<User>();
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Protected { get; set; }

        public bool Deleted { get; set; }

        [Display(Name = "Organization")]
        public Guid? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        [Display(Name = "Organization")]
        public virtual Organization Organization { get; set; }
        public ICollection<User> Users { get; set; }

        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public bool UseSsl { get; set; }
        public string HostName { get; set; }
        public int? Port { get; set; }
        public float? MaxImageFileSize { get; set; }
        public string AcceptedImageFileTypes { get; set; }
        public float? MaxVideoFileSize { get; set; }
        public string AcceptedVideoFileTypes { get; set; }
        public int? PageSize { get; set; }
        public ICollection<Incidence> AllocatedIncidences { get; set; }
        //public ICollection<Incidence> AssignedDepartmentIncidences { get; set; }
        //public ICollection<Incidence> ReporterDepartmentIncidences { get; set; }

        public SMSServiceProvider? SmsServiceProvider { get; set; }
        public string EmailSenderName { get; set; }
        public string EmailSendersEmail { get; set; }
        public string EmailSenderPassword { get; set; }
        public string EmailRecipientAddresses { get; set; }

        public bool SendSMS { get; set; }
        public bool ActivateEmailSenderSettings { get; set; }
        public bool ActivateFileSettings { get; set; }

        public string SMSRecipientNumbers { get; set; }

        public string SMSSenderName { get; set; }
    }
}
