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
    public class UserDeployment: BaseModel, IPseudoDeletable, IEditLoggable, ICreateLoggable, IProtectable
    {        
        [Display(Name = "User")]
        public Guid? UserId { get; set; }
        [ForeignKey("UserId")]
        [Display(Name = "User")]
        public virtual User User { get; set; }

        [Display(Name = "Area")]
        public Guid? AreaOfDeploymentId { get; set; }
        [ForeignKey("AreaOfDeploymentId")]
        [Display(Name = "Area")]
        public virtual Area AreaOfDeployment{ get; set; }

        [ForeignKey("CityOfDeploymentId")]
        [Display(Name = "City")]
        public virtual City CityOfDeployment { get; set; }
        [Display(Name = "City")]
        public Guid? CityOfDeploymentId { get; set; }

        [ForeignKey("StateOfDeploymentId")]
        [Display(Name = "State")]
        public virtual State StateOfDeployment { get; set; }
        [Display(Name = "State")]
        public Guid? StateOfDeploymentId { get; set; }

        [ForeignKey("CountryOfDeploymentId")]
        [Display(Name = "Country")]
        public virtual Country CountryOfDeployment { get; set; }
        [Display(Name = "Country")]
        public Guid? CountryOfDeploymentId { get; set; }

        [Display(Name = "Department")]
        public Guid? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        [Display(Name = "Department")]
        public virtual OrganizationDepartment UserDepartment { get; set; }

        public DateTime? DateOfDeployment { get; set; }

        public DateTime? DateOfSignOff { get; set; }

        public bool CurrentDeployment { get; set; }

        public bool Protected { get; set; }

        public bool Deleted { get; set; }

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
    }
}
