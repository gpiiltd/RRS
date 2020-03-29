using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using IRS.DAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace IRS.DAL.Infrastructure
{
    public class UserDeploymentRepository : IUserDeploymentRepository
    {
        private ApplicationDbContext _context;
        public UserDeploymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<UserDeployment> GetUsersDeploymentInfo(Guid id)
        {
            var userdeployInfo = await _context.UserDeployment
                .FirstOrDefaultAsync(x => x.UserId == id);

            return userdeployInfo;
        }
    }
}
