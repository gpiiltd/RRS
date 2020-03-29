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
    public interface IOrganizationRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<Organization> GetOrganization(Guid? id);
        bool CheckOrganizationCode(string Code);
        Task<IEnumerable<Organization>> GetOrganizations();
        Task<QueryResult<OrganizationViewModel>> GetOrganizationList(OrganizationQuery queryObj);
    }
}
