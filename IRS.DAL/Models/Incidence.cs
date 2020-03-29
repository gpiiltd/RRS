using IRS.DAL.ModelInterfaces;
using IRS.DAL.Models.Identity;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRS.DAL.Models
{
    public enum ReporterFeedbackRatings
    {
        VeryBad,
        Bad,
        Satisfactory,
        Good,
        Perfect
    }

    public class Incidence: BaseModel, IEditLoggable, ICreateLoggable, IPseudoDeletable, IProtectable
    {
        public Incidence()
        {
            Medias = new HashSet<Media>();
        }

        [MaxLength(FieldLenght.LongCodeLength)]
        public string Code { get; set; }

        [MaxLength(FieldLenght.TitleLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(FieldLenght.ShortNoteLength)]
        public string Description { get; set; }

        [MaxLength(FieldLenght.DescriptionLength)]
        public string Comment { get; set; }

        [MaxLength(FieldLenght.ShortNoteLength)]
        public string RouteCause { get; set; }

        [Display(Name = "Area")]
        public Guid? AreaId { get; set; }
        [ForeignKey("AreaId")]
        [Display(Name = "Area")]
        public virtual Area Area { get; set; }        

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

        [Required]
        public string Address { get; set; }

        public double? ReportedIncidenceLatitude { get; set; }
        public double? ReportedIncidenceLongitude { get; set; }

        [MaxLength(FieldLenght.ShortNoteLength)]
        public string Suggestion { get; set; }

        public Guid? AssignerId { get; set; }
        public virtual User Assigner { get; set; }

        public Guid? AssigneeId { get; set; }
        public virtual User Assignee { get; set; }

        [Display(Name = "Date created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Created by")]
        public Guid CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [Display(Name = "Edited on")]
        public DateTime? DateEdited { get; set; }

        [Display(Name = "Edited by")]
        public Guid? EditedByUserId { get; set; }

        [ForeignKey("EditedByUserId")]
        public virtual User EditedByUser { get; set; }

        [Display(Name = "Assigned Organization")]
        public Guid? AssignedOrganizationId { get; set; }
        [ForeignKey("AssignedOrganizationId")]
        public virtual Organization AssignedOrganization { get; set; }

        [Display(Name = "Assigned Department")]
        public Guid? AssignedDepartmentId { get; set; }

        [ForeignKey("AssignedDepartmentId")]
        public virtual OrganizationDepartment AssignedDepartment { get; set; }

        [MaxLength(FieldLenght.UserNameLength)]
        public string ReporterName { get; set; }

        [MaxLength(FieldLenght.UserNameLength)]
        public string ReporterEmail { get; set; }

        [MaxLength(FieldLenght.DescriptionLength)]
        public string ReporterFirstResponderAction { get; set; }

        public int? ReporterFeedbackRating { get; set; }
        public int? ManagerFeedbackRating { get; set; }

        //[ForeignKey("ReporterDepartmentId")]
        //public virtual OrganizationDepartment ReportersDepartment { get; set; } 
        public Guid? ReporterDepartmentId { get; set; }

        [Display(Name = "Incidence Type")]
        public Guid? IncidenceTypeId { get; set; }
        [ForeignKey("IncidenceTypeId")]
        public virtual IncidenceType IncidenceTypes { get; set; }

        [Display(Name = "Incidence Status")]
        public Guid? IncidenceStatusId { get; set; }
        [ForeignKey("IncidenceStatusId")]
        public virtual IncidenceStatus IncidenceStatuses { get; set; }

        [Display(Name = "Resolution Date")]
        public DateTime? ResolutionDate { get; set; }

        public bool Protected { get; set; }

        public bool Deleted { get; set; }

        [Display(Name = "Organization")]
        public Guid? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        [Display(Name = "Organization")]
        public virtual Organization Organization { get; set; }

        public override string ToString()
        {
            return Title;
        }

        public ICollection<Media> Medias { get; set; }
    }
}
