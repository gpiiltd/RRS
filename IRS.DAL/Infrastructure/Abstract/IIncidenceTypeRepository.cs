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
    public interface IIncidenceTypeRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        bool CheckIncidenceType(string Name, Guid? OrganizationId);
        Task<IncidenceType> GetIncidenceType(Guid id);
        Task<IEnumerable<IncidenceType>> GetIncidenceTypes();
        Task<QueryResult<IncidenceTypeViewModel>> GetIncidenceTypeList(IncidenceTypeQuery queryObj);
    }
}
