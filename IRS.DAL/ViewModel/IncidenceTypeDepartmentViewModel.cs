using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.ViewModel
{
    public class IncidenceTypeDepartmentViewModel
    {
        public Guid? Id { get; set; }
        public int SerialNumber { get; set; }
        public Guid? IncidenceTypeId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? OrganizationId { get; set; }
        public string DepartmentName { get; set; }
        public string IncidenceTypeName { get; set; }
        public string OrganizationName { get; set; }
    }
}
