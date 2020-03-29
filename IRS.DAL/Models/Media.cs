using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRS.DAL.Models
{
    public enum FileUploadChannels
    {
        incidencesReportedOnMobile,
        incidencesResolvedOnWeb
    }

    public class Media: BaseModel
    {
        [Required]
        [StringLength(FieldLenght.FileNameLength)]
        public string FileName { get; set; }

        [StringLength(FieldLenght.UrlLength)]
        public string RemoteUri { get; set; }

        public DateTime DateUploaded { get; set; }
        
        public DateTime? DateEdited { get; set; }

        [ForeignKey("IncidenceId")]
        [Display(Name = "Incidence")]
        public virtual Incidence Incidence { get; set; }

        [Display(Name = "Incidence")]
        public Guid? IncidenceId { get; set; }

        [ForeignKey("HazardId")]
        [Display(Name = "Hazard")]
        public virtual Hazard Hazard { get; set; }

        [Display(Name = "Hazard")]
        public Guid? HazardId { get; set; }

        public FileUploadChannels? FileUploadChannel { get; set; }

        public string Description { get; set; }

        public bool IsVideo { get; set; }

        [Display(Name = "Organization")]
        public Guid? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        [Display(Name = "Organization")]
        public virtual Organization Organization { get; set; }

        [Display(Name = "User")]
        public Guid? UserId { get; set; }

        [ForeignKey("UserId")]
        [Display(Name = "User")]
        public virtual User User { get; set; }
    }
}
