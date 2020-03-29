using IRS.DAL.Models.SharedResource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Dtos.UserResources
{
    public enum ReporterFeedbackRatings
    {
        VeryBad,
        Bad,
        Satisfactory,
        Good,
        Perfect
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class UserIncidenceResource
    {
        //ideal for getting user info as well as user incidences, assigned and allocated: Not in use currently
        //public IncidenceResource()
        //{
        //    Photos = new HashSet<PhotoResource>();
        //    Videos = new HashSet<VideoResource>();
        //}

        public string Code { get; set; }

        public string Title { get; set; }

        public string Comment { get; set; }

        public string Description { get; set; }

        public string RouteCause { get; set; }

        public KeyValuePairResource Area { get; set; }

        public KeyValuePairResource City { get; set; }

        public KeyValuePairResource State { get; set; }

        public KeyValuePairResource Country { get; set; }

           
        public KeyValuePairResource Assigner { get; set; }

        public KeyValuePairResource Assignee { get; set; }

        public DateTime? DateCreated { get; set; }

        public Guid? CreatedByUserId { get; set; }

        public DateTime? DateEdited { get; set; }

        public Guid? EditedByUserId { get; set; }

        public OrganizationResource AssignedOrganization { get; set; }

        public string ReporterName { get; set; }

        public string ReporterEmail { get; set; }

        public string ReporterFirstResponderAction { get; set; }

        public int? ReporterFeedbackRating { get; set; }
        public int? ManagerFeedbackRating { get; set; }

        public KeyValuePairResource IncidenceType { get; set; }

        public KeyValuePairResource IncidenceStatus { get; set; }

        public DateTime? ResolutionDate { get; set; }

        //public ICollection<PhotoResource> Photos { get; set; }

        //public ICollection<VideoResource> Videos { get; set; }
    }
}
