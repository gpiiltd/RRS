using IRS.API.Dtos.SharedResource;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IRS.API.Dtos.HazardResources
{
    public class SaveHazardResource: EventResource
    {
        public bool Protected { get; set; }
        public bool Deleted { get; set; }
        public double? ReportedLatitude { get; set; }
        public double? ReportedLongitude { get; set; }
        public IFormFile file { get; set; }
    }
}
