using System;
using System.Collections.Generic;
using System.Text;
using IRS.DAL.Models.SharedResource;

namespace IRS.API.Dtos.IncidenceResources
{
    public class IncidenceTypeDepartmentDto
    {
        public Guid? Id { get; set; }
        public int SerialNumber { get; set; }
        public Guid? IncidenceTypeId { get; set; }

        public virtual KeyValuePairResource IncidenceType { get; set; }

        public Guid? DepartmentId { get; set; }

        public virtual KeyValuePairResource Department { get; set; }

        public Guid? OrganizationId { get; set; }

        public virtual KeyValuePairResource Organization { get; set; }
    }
}
