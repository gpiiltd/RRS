using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Dtos.SharedResource
{
    public class EventResource
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

        public string address { get; set; }

        //make nullable if not used in code generation. otherwise causes creation issues
        
        public string Suggestion { get; set; }

        public Guid? AssignerId { get; set; }

        public Guid? AssigneeId { get; set; }

        public Guid? AssignedOrganizationId { get; set; }

        public Guid? AssignedDepartmentId { get; set; }

        public DateTime? DateCreated { get; set; }

        public Guid? CreatedByUserId { get; set; }

        public DateTime? DateEdited { get; set; }

        public Guid? EditedByUserId { get; set; }
        public Guid? OrganizationId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string ReporterName { get; set; }
        public string ReporterEmail { get; set; }
        public string ReporterFirstResponderAction { get; set; }
        public int? ReporterFeedbackRating { get; set; }
        public int? ManagerFeedbackRating { get; set; }
        public Guid? IncidenceStatusId { get; set; }
        public DateTime? ResolutionDate { get; set; }
    }
}
