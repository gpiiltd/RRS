using IRS.DAL.Models.SharedResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Dtos
{
    public class OrganizationKeyValueResource : KeyValuePairResource
    {
        public OrganizationKeyValueResource()
        {
            Departments = new HashSet<DepartmentKeyValueResource>();
        }

        public ICollection<DepartmentKeyValueResource> Departments { get; set; }

    }
}
