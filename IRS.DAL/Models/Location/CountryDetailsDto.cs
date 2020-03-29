using IRS.DAL.Models;
using IRS.DAL.Models.Shared;
using IRS.DAL.Models.SharedResource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DAL.Models.OrganizationAndDepartments
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CountryDetailsDto
    {
        public Guid? Id { get; set; }
        public int SerialNumber { get; set; }
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //public virtual Country Country { get; set; }
        public Guid? CountryId { get; set; }
    }
}
