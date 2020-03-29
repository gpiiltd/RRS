using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DAL.Models.QueryResources.Incidence
{
    public class IncidenceDashboardQuery
    {
        public Guid? OrganizationId { get; set; }
        public Guid? IncidenceStatusId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
