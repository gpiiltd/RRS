using System;
using System.Collections.Generic;
using System.Text;
using IRS.DAL.Models.SharedResource;

namespace IRS.API.Dtos.IncidenceResources
{
    public class IncidenceStatusDto : KeyValuePairResource
    {
        public int SerialNumber { get; set; }
        public string Description { get; set; }
    }
}
