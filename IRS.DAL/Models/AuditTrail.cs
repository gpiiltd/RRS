using IRS.DAL.Models.Identity;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IRS.DAL.Models
{
    public class AuditTrail: BaseModel
    {
        public string EventAction { get; set; }
        public string Controller { get; set; }
        public string Description { get; set; }
        public DateTime DateHappened { get; set; }
        public string ClientIP { get; set; }
        public string ClientUserAgent { get; set; }
        public string RecordData { get; set; }

        [Display(Name = "User")]
        public Guid? UserId { get; set; }
        [ForeignKey("UserId")]
        [Display(Name = "User")]
        public virtual User User { get; set; }

        [Display(Name = "Organization")]
        public Guid? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        [Display(Name = "Organization")]
        public virtual Organization Organization { get; set; }
    }
}
