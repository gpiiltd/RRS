using IRS.API.Dtos.SharedResource;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IRS.API.Dtos.IncidenceResources
{
    public class SaveIncidenceResource: EventResource
    {
        public Guid? IncidenceTypeId { get; set; }
        //public Guid? IncidenceStatusId { get; set; }
        public bool Protected { get; set; }

        public bool Deleted { get; set; }
        public IFormFile file { get; set; }

        public double? ReportedIncidenceLatitude { get; set; }
        public double? ReportedIncidenceLongitude { get; set; }
    }
}
