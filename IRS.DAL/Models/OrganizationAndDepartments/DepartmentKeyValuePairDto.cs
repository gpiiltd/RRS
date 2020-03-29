using IRS.DAL.Models.Identity;
using IRS.DAL.Models.Shared;
using IRS.DAL.Models.SharedResource;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.Models.OrganizationAndDepartments
{
    public class DepartmentKeyValuePairDto: KeyValuePairResource
    {
        public Guid? Id { get; set; }
        public DepartmentKeyValuePairDto()
        {
            Users = new HashSet<KeyValuePairResource>();
        }

        public ICollection<KeyValuePairResource> Users { get; set; }
    }
}
