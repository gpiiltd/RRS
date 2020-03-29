using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IRS.API.Dtos.IncidenceResources
{
    public class SaveIncidenceOnMobileResource
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }

        public string RouteCause { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? CityId { get; set; }

        public Guid? StateId { get; set; }
        public Guid? CountryId { get; set; }

        //make nullable if not used in code generation. otherwise causes creation issues
        public double ReportedIncidenceLatitude { get; set; }
        public double ReportedIncidenceLongitude { get; set; }

        public string Suggestion { get; set; }

        public Guid? AssignerId { get; set; }

        public Guid? AssigneeId { get; set; }

        public DateTime? DateCreated { get; set; }

        public Guid? CreatedByUserId { get; set; }

        public DateTime? DateEdited { get; set; }

        public Guid? EditedByUserId { get; set; }
        public Guid? OrganizationId { get; set; }
        public string ReporterName { get; set; }
        public string ReporterEmail { get; set; }
        public string ReporterFeedbackComment { get; set; }
        public int? ReporterFeedbackRating { get; set; }
        public int? ManagerFeedbackRating { get; set; }
        public Guid? IncidenceTypeId { get; set; }
        public Guid? IncidenceStatusId { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public bool Protected { get; set; }

        public bool Deleted { get; set; }

        public IFormFile file { get; set; }
    }
}
