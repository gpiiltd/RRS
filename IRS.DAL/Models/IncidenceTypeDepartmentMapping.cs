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
    public class IncidenceTypeDepartmentMapping : BaseModel
    {
        [Display(Name = "Incidence Type")]
        public Guid? IncidenceTypeId { get; set; }

        [ForeignKey("IncidenceTypeId")]
        [Display(Name = "Incidence Type")]
        public virtual IncidenceType IncidenceType { get; set; }

        [Display(Name = "Department")]
        public Guid? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        [Display(Name = "Department")]
        public virtual OrganizationDepartment Department { get; set; }

        [Display(Name = "Organization")]
        public Guid? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        [Display(Name = "Organization")]
        public virtual Organization Organization { get; set; }
    }
}
