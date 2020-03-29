using IRS.DAL.Helpers;
using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using IRS.DAL.Models.QueryResource.Incidence;
using IRS.DAL.Models.QueryResources.Incidence;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.Models.Shared;
using IRS.DAL.Models.SharedResource;
using IRS.DAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static IRS.DAL.ViewModel.IncidenceDashboardReportViewModel;

namespace IRS.DAL.Infrastructure
{
    public class IncidenceReportRepository : IIncidenceReportRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUserRepository _userRepo;

        public IncidenceReportRepository(ApplicationDbContext context, IUserRepository userRepo)
        {
            this.context = context;
            _userRepo = userRepo;
        }

        public async Task<QueryResultDashboard<IncidenceDashboardMonthlyViewModel>> GetIncidenceReportForDashboard(IncidenceDashboardQueryResource queryObj)
        {
            var result = new QueryResultDashboard<IncidenceDashboardMonthlyViewModel>();
            var items = new List<IncidenceDashboardMonthlyViewModel>();
            var queryParam = new IncidenceDashboardQuery();
            var currentUser = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());

            //get Open incidence monthly data
            queryParam.IncidenceStatusId = GlobalFields.OpenIncidenceStatus;
            queryParam.OrganizationId = queryObj.OrganizationId == null 
                ? currentUser != null && currentUser.UserName != "Admin" ? currentUser.OrganizationId : Guid.Empty
                : queryObj.OrganizationId;
            queryParam.FromDate = new DateTime(queryObj.Year.Value, 1, 1);
            queryParam.ToDate = new DateTime(queryObj.Year.Value, 12, 31, 23, 59, 59);

            IncidenceDashboardMonthlyViewModel openIncidences = await GetIncidenceDashboardReport(queryParam);
            items.Add(openIncidences);

            //get Closed incidence monthly data
            queryParam.IncidenceStatusId = GlobalFields.ClosedIncidenceStatus;

            IncidenceDashboardMonthlyViewModel closedIncidences = await GetIncidenceDashboardReport(queryParam);
            items.Add(closedIncidences);

            //get Under-Review incidence monthly data
            queryParam.IncidenceStatusId = GlobalFields.UnderReviewIncidenceStatus;

            

            IncidenceDashboardMonthlyViewModel underReviewIncidences = await GetIncidenceDashboardReport(queryParam);
            items.Add(underReviewIncidences);
            result.Items = items;
            //chosen year stats
            result.NewItems = await GetIncidenceCount(queryParam.OrganizationId, GlobalFields.NewIncidenceStatus, queryObj.Year);
            result.OpenItems = await GetIncidenceCount(queryParam.OrganizationId, GlobalFields.OpenIncidenceStatus, queryObj.Year);
            result.ClosedItems = await GetIncidenceCount(queryParam.OrganizationId, GlobalFields.ClosedIncidenceStatus, queryObj.Year);
            result.ReopenedItems = await GetIncidenceCount(queryParam.OrganizationId, GlobalFields.ReOpenedIncidenceStatus, queryObj.Year);
            result.ResolvedItems = await GetIncidenceCount(queryParam.OrganizationId, GlobalFields.ResolvedIncidenceStatus, queryObj.Year);
            result.UnderReviewItems = await GetIncidenceCount(queryParam.OrganizationId, GlobalFields.UnderReviewIncidenceStatus, queryObj.Year);

            //all time stats
            result.AllOpenItems = await GetIncidenceCount(queryParam.OrganizationId, GlobalFields.OpenIncidenceStatus, null);
            result.AllClosedItems = await GetIncidenceCount(queryParam.OrganizationId, GlobalFields.ClosedIncidenceStatus, null);
            result.AllReopenedItems = await GetIncidenceCount(queryParam.OrganizationId, GlobalFields.ReOpenedIncidenceStatus, null);
            result.AllResolvedItems = await GetIncidenceCount(queryParam.OrganizationId, GlobalFields.ResolvedIncidenceStatus, null);
            result.AllUnderReviewItems = await GetIncidenceCount(queryParam.OrganizationId, GlobalFields.UnderReviewIncidenceStatus, null);
            result.AllNewItems = await GetIncidenceCount(queryParam.OrganizationId, GlobalFields.NewIncidenceStatus, null);

            return result;
        }

        public async Task<IncidenceDashboardMonthlyViewModel> GetIncidenceDashboardReport(IncidenceDashboardQuery queryObj)
        {
            //List<IncidenceDashboardMonthlyViewModel> result;
            var result = new IncidenceDashboardMonthlyViewModel();

            //get all incidences for the current user or all incidences if admin user
            var query = context.Incidences
                .Where(x => x.DateEdited >= queryObj.FromDate && x.DateEdited <= queryObj.ToDate)
                .Where(x => x.IncidenceStatusId == queryObj.IncidenceStatusId)
                .Where(x => !x.Deleted);

            if (queryObj.OrganizationId != null && queryObj.OrganizationId != Guid.Empty)
            {
                query = query.Where(x => x.OrganizationId == queryObj.OrganizationId);

                var incidences = query.Select(x => new IncidenceDashboardViewModel
                {
                    OrganizationId = x.OrganizationId.Value,
                    OrganizationName = x.Organization.CompanyName,
                    IncidenceStatusId = x.IncidenceStatusId.Value,
                    DateEdited = x.DateEdited.Value
                }).AsQueryable();

                //included ToList method to force records into memory to allow GroupBy expr evaluation as LINQ queries are no longer evaluated on the client automatically . See https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.0/breaking-changes#linq-queries-are-no-longer-evaluated-on-the-client
                var q = incidences.ToList().GroupBy(key => new { key.OrganizationName })
                    .Select(x => new IncidenceDashboardMonthlyViewModel
                    {
                    //OrganizationName = x.Key.OrganizationName,
                    Month1 = x.Count(s => s.DateEdited.Month == 1),
                        Month2 = x.Count(s => s.DateEdited.Month == 2),
                        Month3 = x.Count(s => s.DateEdited.Month == 3),
                        Month4 = x.Count(s => s.DateEdited.Month == 4),
                        Month5 = x.Count(s => s.DateEdited.Month == 5),
                        Month6 = x.Count(s => s.DateEdited.Month == 6),
                        Month7 = x.Count(s => s.DateEdited.Month == 7),
                        Month8 = x.Count(s => s.DateEdited.Month == 8),
                        Month9 = x.Count(s => s.DateEdited.Month == 9),
                        Month10 = x.Count(s => s.DateEdited.Month == 10),
                        Month11 = x.Count(s => s.DateEdited.Month == 11),
                        Month12 = x.Count(s => s.DateEdited.Month == 12),
                    });

                result = q.FirstOrDefault();
            } else
            {
                var incidences = query.Select(x => new IncidenceDashboardViewModel
                {
                    IncidenceStatusId = x.IncidenceStatusId.Value,
                    DateEdited = x.DateEdited.Value
                }).AsQueryable();

                //included ToList method to force records into memory to allow GroupBy expr evaluation as LINQ queries are no longer evaluated on the client automatically . See https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.0/breaking-changes#linq-queries-are-no-longer-evaluated-on-the-client
                var q = incidences.ToList().GroupBy(key => new { key.IncidenceStatusId })
                    .Select(x => new IncidenceDashboardMonthlyViewModel
                    {
                        //OrganizationName = x.Key.OrganizationName,
                        Month1 = x.Count(s => s.DateEdited.Month == 1),
                        Month2 = x.Count(s => s.DateEdited.Month == 2),
                        Month3 = x.Count(s => s.DateEdited.Month == 3),
                        Month4 = x.Count(s => s.DateEdited.Month == 4),
                        Month5 = x.Count(s => s.DateEdited.Month == 5),
                        Month6 = x.Count(s => s.DateEdited.Month == 6),
                        Month7 = x.Count(s => s.DateEdited.Month == 7),
                        Month8 = x.Count(s => s.DateEdited.Month == 8),
                        Month9 = x.Count(s => s.DateEdited.Month == 9),
                        Month10 = x.Count(s => s.DateEdited.Month == 10),
                        Month11 = x.Count(s => s.DateEdited.Month == 11),
                        Month12 = x.Count(s => s.DateEdited.Month == 12),
                    });

                result = q.FirstOrDefault();
            }

            return result;
        }

        public async Task<int> GetIncidenceCount(Guid? OrganizationId, Guid IncidenceStatusId, int? Year)
        {
            var query = context.Incidences
                         .Where(x => x.IncidenceStatusId == IncidenceStatusId && !x.Deleted);

            //filter with the user's organization if not the Admin user
            if (OrganizationId != null && OrganizationId != Guid.Empty)
            {
                query = query.Where(x => x.OrganizationId == OrganizationId);
            }
            //New status incidences dont have EditedDate. Use created date to count number of incidences for the year in query otherwise use EditedDate
            if (IncidenceStatusId == GlobalFields.NewIncidenceStatus)
            {
                if (Year != null)
                    query = query.Where(x => x.DateCreated.Year == Year);
            }
            if (Year != null && IncidenceStatusId != GlobalFields.NewIncidenceStatus)
                query = query.Where(x => x.DateEdited.Value.Year == Year);

            return await query.CountAsync();
        }
    }
}
