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
    public interface IStateRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<State> GetState(Guid? id);
        Task<IEnumerable<State>> GetStates();
        bool CheckStateCode(string Code);
        Task<QueryResult<StateDetailsViewModel>> GetStateList(LocationQuery queryObj);
    }
}
