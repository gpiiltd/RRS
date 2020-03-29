using IRS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.ViewModel
{
    public class IncidenceViewModel
    {
        public Guid? Id { get; set; }

        public int SerialNumber { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Comment { get; set; }

        public string RouteCause { get; set; }

        public Guid? AreaId { get; set; }

        public string AreaName { get; set; }

        public Guid? CityId { get; set; }

        public string CityName { get; set; }

        public Guid? StateId { get; set; }

        public string StateName { get; set; }

        public Guid? CountryId { get; set; }

        public string CountryName { get; set; }

        public double? ReportedIncidenceLatitude { get; set; }

        public double? ReportedIncidenceLongitude { get; set; }

        public string Suggestion { get; set; }

        public Guid? AssignerId { get; set; }

        public string AssignerName { get; set; }

        public Guid? AssigneeId { get; set; }

        public string AssigneeName { get; set; }

        public string AssigneeJobTitle { get; set; }

        public DateTime? DateCreated { get; set; }

        public Guid? CreatedByUserId { get; set; }

        public DateTime? DateEdited { get; set; }

        public Guid? EditedByUserId { get; set; }

        public Guid? AssignedOrganizationId { get; set; }

        public string AssignedOrganizationName { get; set; }

        public Guid? AssignedDepartmentId { get; set; }

        public string AssignedDepartmentName { get; set; }

        public string ReporterName { get; set; }

        public string ReporterEmail { get; set; }

        public string ReporterFirstResponderAction { get; set; }

        public int? ReporterFeedbackRating { get; set; }
        public int? ManagerFeedbackRating { get; set; }

        public Guid? ReporterDepartmentId { get; set; }

        public Guid? IncidenceTypeId { get; set; }

        public string IncidenceTypeName { get; set; }

        public Guid? IncidenceStatusId { get; set; }

        public string IncidenceStatusName { get; set; }

        public DateTime? ResolutionDate { get; set; }

        //reported via mobile app
        public ICollection<Media> Photos { get; set; }

        //reported via mobile app
        public ICollection<Media> Videos { get; set; }
    }
}
