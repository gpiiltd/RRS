using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.Models.QueryResources.Users;
using IRS.DAL.ViewModel;

namespace IRS.DAL.Infrastructure.Abstract
{
    public interface IUserRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<IEnumerable<User>> GetUsers();
        Task<QueryResult<UserDetailsViewModel>> GetUserList(UsersQuery queryObj);
        Task<User> GetUser(Guid? id);
        Task<UserDeployment> GetUserDeploymentData(Guid? id);
        Task<UserDetailsViewModel> GetUserFullData(Guid id);
        bool CheckUser(string Username);
        Task<QueryResult<UsersRolesViewModel>> GetUsersWithRoles(UsersRolesQuery queryObj);
        Guid GetLoggedInUserId();
    }
}
