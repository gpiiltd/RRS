using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IRS.DAL.Models;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.ViewModel;

namespace IRS.DAL.Infrastructure.Abstract
{
    public interface IDepartmentRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        bool CheckDepartmentCode(string Code, Guid? OrganizationId);
        Task<OrganizationDepartment> GetDepartment(Guid? id);
        Task<IEnumerable<OrganizationDepartment>> GetDepartments();
        Task<QueryResult<DepartmentViewModel>> GetDepartmentList(DepartmentQuery queryObj);
    }
}
