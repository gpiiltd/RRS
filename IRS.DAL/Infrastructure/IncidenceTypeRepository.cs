using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IRS.DAL.Helpers;
using System.Linq;
using IRS.DAL.Models.QueryResource.Incidence;

namespace IRS.DAL.Infrastructure
{
    public class IncidenceTypeRepository : IIncidenceTypeRepository
    {
        private ApplicationDbContext _context;
        private readonly IUserRepository _userRepo;
        public IncidenceTypeRepository(ApplicationDbContext context, IUserRepository userRepo)
        {
            _context = context;
            _userRepo = userRepo;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public bool CheckIncidenceType(string Name, Guid? OrganizationId)
        {
            if (_context.IncidenceTypes.Any(v => v.Name == Name && v.OrganizationId == OrganizationId))
                return true;
            else
                return false;
        }

        public async Task<IncidenceType> GetIncidenceType(Guid id)
        {
            var incidenceType = await _context.IncidenceTypes
                .FirstOrDefaultAsync(x => x.Id == id);

            return incidenceType;
        }

        public async Task<IEnumerable<IncidenceType>> GetIncidenceTypes()
        {
            var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
            var query = _context.IncidenceTypes
                .AsQueryable();

            if (user != null && user.UserName != "Admin")
                query = query.Where(x => x.OrganizationId == user.OrganizationId);

            return await query.ToListAsync();
        }

        public async Task<QueryResult<IncidenceTypeViewModel>> GetIncidenceTypeList(IncidenceTypeQuery queryObj)
        {
            var result = new QueryResult<IncidenceTypeViewModel>();
            var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
            var query = (from it in _context.IncidenceTypes
                         select new 
                         {
                             Id = it.Id,
                             Name = it.Name,
                             Description = it.Description,
                             OrganizationId = it.OrganizationId,
                             Deleted = it.Deleted
                         })
                         .Where(x => user.UserName != "Admin" ? x.OrganizationId == user.OrganizationId && x.Deleted != true : x.Deleted != true)
                         .ToList().Select((s, index) => new IncidenceTypeViewModel()
                         {
                             SerialNumber = index + 1,
                             Id = s.Id,
                             Name = s.Name,
                             Description = s.Description,
                             OrganizationId = s.OrganizationId
                         })
                         .AsQueryable();

            if (!string.IsNullOrEmpty(queryObj.Name))
                query = query.Where(v => v.Name.Contains(queryObj.Name) | v.Description.Contains(queryObj.Name));

            //string key of the dictionary must be equal the column title in the ui. Value of the dictionary must be a valid column in db/query
            var columnsMap = new Dictionary<string, Expression<Func<IncidenceTypeViewModel, object>>>()
            {
                ["Name"] = v => v.Name,
                ["Description"] = v => v.Description
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObj);

            result.Items = query.ToList();

            return result;
        }

    }
}
