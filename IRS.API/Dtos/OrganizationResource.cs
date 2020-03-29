using IRS.DAL.Models.Shared;
using IRS.DAL.Models.SharedResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Dtos
{
    public class OrganizationResource
    {
        public Guid? Id { get; set; }
        public string CompanyName { get; set; }
        public string RegistrationNo { get; set; }
        public string BusinessCategory { get; set; }
        public string Code { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactMiddleName { get; set; }
        public string ContactLastName { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string OfficeAddress { get; set; }
        public string BrandLogo { get; set; }

        public DateTime DateofEst { get; set; }
        public string Comment { get; set; }

        public KeyValuePairResource Area { get; set; }

        public KeyValuePairResource City { get; set; }

        public KeyValuePairResource State { get; set; }

        public KeyValuePairResource Country { get; set; }

        public ICollection<KeyValuePairResource> AllocatedIncidences { get; set; }
    }
}
