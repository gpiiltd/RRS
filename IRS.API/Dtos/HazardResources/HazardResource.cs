using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using IRS.DAL.Models.SharedResource;

namespace IRS.API.Dtos.HazardResources
{
    public class HazardResource
    {
        public HazardResource()
        {
            Photos = new HashSet<KeyValuePairResource>();
            Videos = new HashSet<KeyValuePairResource>();
        }

        public Guid? Id { get; set; }

        public int SerialNumber { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }

        public string RouteCause { get; set; }

        public Guid? AreaId { get; set; }

        public KeyValuePairResource Area { get; set; }

        public Guid? CityId { get; set; }

        public KeyValuePairResource City { get; set; }

        public Guid? StateId { get; set; }

        public KeyValuePairResource State { get; set; }

        public Guid? CountryId { get; set; }

        public string address { get; set; }

        public KeyValuePairResource Country { get; set; }

        public double ReportedLatitude { get; set; }

        public double ReportedLongitude { get; set; }

        public string Suggestion { get; set; }

        public Guid? AssignerId { get; set; }

        public KeyValuePairResource Assigner { get; set; }

        public Guid? AssigneeId { get; set; }

        public KeyValuePairResource Assignee { get; set; }

        public string AssigneeJobTitle { get; set; }

        public DateTime? DateCreated { get; set; }

        public Guid? CreatedByUserId { get; set; }

        public DateTime? DateEdited { get; set; }

        public Guid? EditedByUserId { get; set; }

        public Guid? OrganizationId { get; set; }

        //public OrganizationResource AssignedOrganization { get; set; }
        public KeyValuePairResource AssignedOrganization { get; set; }

        public Guid? AssignedDepartmentId { get; set; }
        public Guid? AssignedOrganizationId { get; set; }

        public KeyValuePairResource AssignedDepartment { get; set; }

        public string ReporterName { get; set; }

        public string ReporterEmail { get; set; }

        public string ReporterFirstResponderAction { get; set; }

        public Guid? ReporterDepartmentId { get; set; }

        public int? ReporterFeedbackRating { get; set; }
        public int? ManagerFeedbackRating { get; set; }

        public Guid? IncidenceStatusId { get; set; }

        public KeyValuePairResource IncidenceStatus { get; set; }

        public DateTime? ResolutionDate { get; set; }

        public ICollection<KeyValuePairResource> Photos { get; set; }

        public ICollection<KeyValuePairResource> Videos { get; set; }
    }
}
