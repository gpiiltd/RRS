using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IRS.DAL.Models;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.QueryResource.Incidence;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.ViewModel;

namespace IRS.DAL.Infrastructure.Abstract
{
    public interface IIncidenceStatusRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        bool CheckIncidenceStatus(string Name);
        Task<IncidenceStatus> GetIncidenceStatus(Guid id);
        Task<IEnumerable<IncidenceStatus>> GetIncidenceStatuses();
        Task<QueryResult<IncidenceStatusViewModel>> GetIncidenceStatusList(IncidenceStatusQuery queryObj);
    }
}
