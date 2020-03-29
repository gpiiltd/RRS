using IRS.DAL.ModelInterfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRS.DAL.Models.Identity
{
    public enum Gender
    {
        Male,
        Female,
    }

    public enum PreferredContactMethod
    {
        Email,
        SMS,
        Phone
    }

    public class User: IdentityUser<Guid>, IDbModel, IPseudoDeletable, IProtectable
    {
        public User()
        {
            Medias = new HashSet<Media>();
        }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }

        public string Email1 { get; set; }
        public string Email2 { get; set; }

        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        public string StaffNo { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime? LastActive { get; set; }
        public string Introduction { get; set; }

        [Display(Name = "Area of Origin")]
        public Guid? AreaOfOriginId { get; set; }
        [ForeignKey("AreaOfOriginId")]
        [Display(Name = "Area")]
        public virtual Area AreaOfOrigin { get; set; }

        [ForeignKey("CityOfOriginId")]
        [Display(Name = "City of Origin")]
        public virtual City CityOfOrigin { get; set; }
        [Display(Name = "City of Origin")]
        public Guid? CityOfOriginId { get; set; }

        [ForeignKey("StateOfOriginId")]
        [Display(Name = "State of Origin")]
        public virtual State StateOfOrigin { get; set; }
        [Display(Name = "State of Origin")]
        public Guid? StateOfOriginId { get; set; }

        [ForeignKey("CountryOfOriginId")]
        [Display(Name = "Country of Origin")]
        public virtual Country CountryOfOrigin { get; set; }
        [Display(Name = "Country of Origin")]
        public Guid? CountryOfOriginId { get; set; }

        public Guid? OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization UserOrganization { get; set; }

        public Guid? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual OrganizationDepartment UserDepartment { get; set; }

        public Gender Gender { get; set; }
        public PreferredContactMethod PreferredContactMethod { get; set; }
        public string PhotoUrl { get; set; }

        public ICollection<Incidence> AssignedIncidences { get; set; }
        public ICollection<Incidence> AllocatedIncidences { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
        public ICollection<Incidence> CreatedIncidences { get; set; }
        public ICollection<Incidence> EditedIncidences { get; set; }

        public bool Protected { get; set; }

        public bool Deleted { get; set; }

        [Display(Name = "Date created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Created by")]
        public Guid CreatedByUserId { get; set; }

        //Same table Foreign key Duplicate Index problem
        //[ForeignKey(nameof(CreatedByUserId))]
        //public virtual User CreatedByUser { get; set; }

        [Display(Name = "Edited on")]
        public DateTime? DateEdited { get; set; }

        [Display(Name = "Edited by")]
        public Guid? EditedByUserId { get; set; }

        public string toStringField { get; set; }

        public ICollection<Media> Medias { get; set; }

        public bool IsActive { get; set; }

        public string MobileAppLoginPattern { get; set; }

        //Same table Foreign key Duplicate Index problem
        //[ForeignKey(nameof(EditedByUserId))]
        //public virtual User EditedByUser { get; set; }
    }
}