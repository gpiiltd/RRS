using System;
using System.Collections.Generic;
using System.Text;
using IRS.DAL.Models.SharedResource;

namespace IRS.API.Dtos.IncidenceResources
{
    public class IncidenceTypeDto : KeyValuePairResource
    {
        public int SerialNumber { get; set; }
        public Guid? OrganizationId { get; set; }
        public string Description { get; set; }
    }
}
