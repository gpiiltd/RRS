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
    public class IncidenceStatusRepository : IIncidenceStatusRepository
    {
        private ApplicationDbContext _context;
        public IncidenceStatusRepository(ApplicationDbContext context)
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

        public bool CheckIncidenceStatus(string Name)
        {
            if (_context.IncidenceStatuses.Any(v => v.Name == Name))
                return true;
            else
                return false;
        }

        public async Task<IncidenceStatus> GetIncidenceStatus(Guid id)
        {
            var incidenceStatus = await _context.IncidenceStatuses
                .FirstOrDefaultAsync(x => x.Id == id);

            return incidenceStatus;
        }

        public async Task<IEnumerable<IncidenceStatus>> GetIncidenceStatuses()
        {
            var incidenceStatuses = await _context.IncidenceStatuses
                .ToListAsync();

            return incidenceStatuses;
        }

        public async Task<QueryResult<IncidenceStatusViewModel>> GetIncidenceStatusList(IncidenceStatusQuery queryObj)
        {
            var result = new QueryResult<IncidenceStatusViewModel>();

            var query = (from it in _context.IncidenceStatuses
                         select new 
                         {
                             Id = it.Id,
                             Name = it.Name,
                             Description = it.Description
                         })
                         .ToList().Select((s, index) => new IncidenceStatusViewModel()
                         {
                             SerialNumber = index + 1,
                             Id = s.Id,
                             Name = s.Name,
                             Description = s.Description
                         })
                         .AsQueryable();

            if (!string.IsNullOrEmpty(queryObj.Name))
                query = query.Where(v => v.Name.Contains(queryObj.Name) | v.Description.Contains(queryObj.Name));

            //string key of the dictionary must be equal the column title in the ui. Value of the dictionary must be a valid column in db/query
            var columnsMap = new Dictionary<string, Expression<Func<IncidenceStatusViewModel, object>>>()
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
