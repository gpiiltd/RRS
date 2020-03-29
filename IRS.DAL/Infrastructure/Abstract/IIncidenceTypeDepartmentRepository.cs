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
    public interface IIncidenceTypeDepartmentRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        bool CheckIncidenceTypeDepartment(Guid? IncidenceTypeId, Guid? OrganizationId);
        Task<IncidenceTypeDepartmentMapping> GetIncidenceTypeDepartment(Guid? id);
        Task<OrganizationDepartment> GetDepartmentFromIncidenceType(Guid? incidenceTypeId);
        //Task<IEnumerable<IncidenceType>> GetIncidenceTypesDepartments();
        Task<QueryResult<IncidenceTypeDepartmentViewModel>> GetIncidenceTypesDepartmentsList(IncidenceTypeDepartmentQuery queryObj);
        Task<IEnumerable<IncidenceType>> GetUnmappedIncidenceTypes();
    }
}
