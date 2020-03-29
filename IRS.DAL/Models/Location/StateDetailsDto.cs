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
    public class StateDetailsDto
    {
        public Guid? Id { get; set; }
        public int SerialNumber { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string Description { get; set; }
        public string CountryName { get; set; }

        //public virtual Country Country { get; set; }
        public Guid? CountryId { get; set; }
    }
}
