using IRS.DAL.Models;
using IRS.DAL.Models.QueryResource.Incidence;
using IRS.DAL.Models.QueryResources.Incidence;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IRS.DAL.ViewModel.IncidenceDashboardReportViewModel;

namespace IRS.DAL.Infrastructure.Abstract
{
    public interface IIncidenceReportRepository
    {
        Task<QueryResultDashboard<IncidenceDashboardMonthlyViewModel>> GetIncidenceReportForDashboard(IncidenceDashboardQueryResource queryObj);
        Task<IncidenceDashboardMonthlyViewModel> GetIncidenceDashboardReport(IncidenceDashboardQuery queryObj);
        Task<int> GetIncidenceCount(Guid? OrganizationId, Guid IncidenceStatusId, int? Year);
    }
}
