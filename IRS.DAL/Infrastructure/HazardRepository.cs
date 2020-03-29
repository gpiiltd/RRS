using IRS.DAL.Helpers;
using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using IRS.DAL.Models.QueryResource.Hazard;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.Models.SharedResource;
using IRS.DAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IRS.DAL.Infrastructure
{
    public class HazardRepository : IHazardRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUserRepository _userRepo;

        public HazardRepository(ApplicationDbContext context, IUserRepository userRepo)
        {
            this.context = context;
            _userRepo = userRepo;
        }

        public async Task<Hazard> GetHazard(Guid id, bool includeRelated= true)
        {
            if (!includeRelated)
                return await context.Hazards.FindAsync(id);

            return await context.Hazards
              .Include(v => v.Area)
                .Include(vf => vf.City)
                    .Include(v => v.State)
                        .ThenInclude(m => m.Country)
              .Include(i => i.IncidenceStatuses)
              .Include(i => i.AssignedOrganization)
              //.Include(i => i.AssignedDepartment)
              .SingleOrDefaultAsync(v => v.Id == id);
        }

        public void Add(Hazard hazard)
        {
            context.Hazards.Add(hazard);
        }

        public void Remove(Hazard hazard)
        {
            context.Hazards.Remove(hazard);
        }

        public bool CheckHazardCode(string Code)
        {
            if (context.Hazards.Any(i => i.Code == Code && i.Deleted != true))
                return true;
            else
                return false;
        }

        public async Task<QueryResult<HazardViewModel>> GetHazards(HazardQuery queryObj)
        {
            var result = new QueryResult<HazardViewModel>();
            var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());

            //only join to be able to serach incidence by area on server side. i.e use ia.Name rather than inc.Area.Name
            var query = (from inc in context.Hazards
                         .Include(x => x.Medias)
                          select new
                          {
                              Id = inc.Id,
                              Code = inc.Code,
                              Title = inc.Title,
                              Description = inc.Description,
                              Comment = inc.Comment,
                              RouteCause = inc.RouteCause,
                              AreaId = inc.AreaId,
                              CityId = inc.CityId,
                              StateId = inc.StateId,
                              CountryId = inc.CountryId,
                              ReportedLatitude = inc.ReportedLatitude,
                              ReportedLongitude = inc.ReportedLongitude,
                              Suggestion = inc.Suggestion,
                              AssignerId = inc.AssignerId,
                              AssignerName = inc.Assigner.FirstName + " " + inc.Assigner.LastName,
                              AssigneeId = inc.AssigneeId,
                              AssigneeName = inc.Assignee.FirstName + " " + inc.Assignee.LastName,
                              AssigneeJobTitle = inc.Assignee.JobTitle,
                              AssignedDepartmentId = inc.AssignedDepartmentId,
                              AssignedDepartmentName = inc.AssignedDepartment.Name,
                              AssignedOrganizationId = inc.AssignedOrganizationId,
                              AssignedOrganizationName = inc.AssignedOrganization.CompanyName,
                              ReporterName = inc.ReporterName,
                              ReporterEmail = inc.ReporterEmail,
                              ReporterFirstResponderAction = inc.ReporterFirstResponderAction,
                              ReporterFeedbackRating = inc.ReporterFeedbackRating,
                              ManagerFeedbackRating = inc.ManagerFeedbackRating,
                              ReporterDepartmentId = inc.ReporterDepartmentId,
                              IncidenceStatusId = inc.IncidenceStatusId,
                              IncidenceStatusName = inc.IncidenceStatuses.Name,
                              ResolutionDate = inc.ResolutionDate,
                              AreaName = inc.Area.Name,
                              CityName = inc.City.Name,
                              StateName = inc.State.Name,
                              CountryName = inc.Country.Name,
                              OrganizationId = inc.OrganizationId,
                              Deleted = inc.Deleted,
                              DateCreated = inc.DateCreated,
                              CreatedByUserId = inc.CreatedByUserId,
                              DateEdited = inc.DateEdited,
                              EditedByUserId = inc.EditedByUserId,
                              pro = inc.Medias,
                              Photos = (from ind in inc.Medias
                                        where ind.FileUploadChannel == FileUploadChannels.incidencesReportedOnMobile && ind.IsVideo == false
                                        select ind).Distinct().ToList(),
                              Videos = (from ind in inc.Medias
                                        where ind.FileUploadChannel == FileUploadChannels.incidencesReportedOnMobile && ind.IsVideo == true
                                        select ind).Distinct().ToList()
                          }).Where(x => user.UserName != "Admin" ? x.OrganizationId == user.OrganizationId && x.Deleted != true : x.Deleted != true)
                          .OrderByDescending(x => x.DateCreated)
                          .ToList().Select((s, index) => new HazardViewModel()
                         {
                             SerialNumber = index + 1,
                             Id = s.Id,
                             Code = s.Code,
                             Title = s.Title,
                             Description = s.Description,
                             Comment = s.Comment,
                             RouteCause = s.RouteCause,
                             AreaId = s.AreaId,
                             CityId = s.CityId,
                             StateId = s.StateId,
                             CountryId = s.CountryId,
                             ReportedLatitude = s.ReportedLatitude,
                             ReportedLongitude = s.ReportedLongitude,
                             Suggestion = s.Suggestion,
                             AssignerId = s.AssignerId,
                             AssignerName = s.AssignerName,
                             AssigneeId = s.AssigneeId,
                             AssigneeName = s.AssigneeName,
                             AssigneeJobTitle = s.AssigneeJobTitle,
                             AssignedDepartmentId = s.AssignedDepartmentId,
                             AssignedDepartmentName = s.AssignedDepartmentName,
                             AssignedOrganizationId = s.AssignedOrganizationId,
                             AssignedOrganizationName = s.AssignedOrganizationName,
                             ReporterName = s.ReporterName,
                             ReporterEmail = s.ReporterEmail,
                             ReporterFirstResponderAction = s.ReporterFirstResponderAction,
                             ReporterFeedbackRating = s.ReporterFeedbackRating,
                             ManagerFeedbackRating = s.ManagerFeedbackRating,
                             ReporterDepartmentId = s.ReporterDepartmentId,
                             IncidenceStatusId = s.IncidenceStatusId,
                             IncidenceStatusName = s.IncidenceStatusName,
                             ResolutionDate = s.ResolutionDate,
                             AreaName = s.AreaName,
                             CityName = s.CityName,
                             StateName = s.StateName,
                             CountryName = s.CountryName,
                             DateCreated = s.DateCreated,
                             CreatedByUserId = s.CreatedByUserId,
                             DateEdited = s.DateEdited,
                             EditedByUserId = s.EditedByUserId,
                             Photos = s.Photos,
                             Videos = s.Videos
                         })
                         .AsQueryable();

            if (queryObj.AreaId.HasValue)
                query = query.Where(v => v.AreaId == queryObj.AreaId.Value);

            if (queryObj.CityId.HasValue)
                query = query.Where(v => v.CityId == queryObj.CityId.Value);

            var columnsMap = new Dictionary<string, Expression<Func<HazardViewModel, object>>>()
            {
                ["area"] = v => v.AreaName,
                ["city"] = v => v.CityName,
                ["state"] = v => v.StateName,
                ["Country"] = v => v.CountryName,
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = query.Count();

            //query = query.ApplyPaging(queryObj); handled on client

            result.Items = query.ToList();

            return result;

        }
    }
}
