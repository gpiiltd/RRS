using IRS.DAL.Models.SharedResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Dtos
{
    public class DepartmentKeyValueResource: KeyValuePairResource
    {
        public DepartmentKeyValueResource()
        {
            Users = new HashSet<KeyValuePairResource>();
        }

        public ICollection<KeyValuePairResource> Users { get; set; }

    }
}
