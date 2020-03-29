using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IRS.DAL.Models;
using IRS.DAL.ViewModel;

namespace IRS.DAL.Infrastructure.Abstract
{
    public interface IUserDeploymentRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<UserDeployment> GetUsersDeploymentInfo(Guid id);
    }
}
