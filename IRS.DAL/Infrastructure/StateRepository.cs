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

namespace IRS.DAL.Infrastructure
{
    public class StateRepository : IStateRepository
    {
        private ApplicationDbContext _context;
        public StateRepository(ApplicationDbContext context)
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

        public bool CheckStateCode(string Code)
        {
            if (_context.States.Any(v => v.Code == Code))
                return true;
            else
                return false;
        }

        public async Task<State> GetState(Guid? id)
        {
            if (id == Guid.Empty)
                return null;
            var state = await _context.States
                .Include(x => x.Areas)
                .FirstOrDefaultAsync(x => x.Id == id);

            return state;
        }

        public async Task<IEnumerable<State>> GetStates()
        {
            var states = await _context.States
                .Include(x => x.Areas)
                .Include(x => x.Cities)
                .ToListAsync();

            return states;
        }

        public async Task<QueryResult<StateDetailsViewModel>> GetStateList(LocationQuery queryObj)
        {
            var result = new QueryResult<StateDetailsViewModel>();

            var query = (from st in _context.States
                         join co in _context.Countries
                            on st.CountryId equals co.Id
                            into StatesCountry
                         //do left join
                         from sc in StatesCountry.DefaultIfEmpty()
                         select new
                         {
                             Id = st.Id,
                             StateCode = st.Code,
                             Description = st.Description,
                             StateName = st.Name,
                             CountryName = sc.Name,
                             CountryId = sc.Id,
                             Deleted = st.Deleted
                         })
                         .Where(x => x.Deleted != true)
                         .ToList().Select((s, index) => new StateDetailsViewModel()
                         {
                             SerialNumber = index + 1,
                             Id = s.Id,
                             StateCode = s.StateCode,
                             Description = s.Description,
                             StateName = s.StateName,
                             CountryName = s.CountryName,
                             CountryId = s.CountryId
                         })
                         .AsQueryable();

            if (!string.IsNullOrEmpty(queryObj.Name))
                query = query.Where(v => v.StateCode.Contains(queryObj.Name) | v.StateName.Contains(queryObj.Name) | v.Description.Contains(queryObj.Name));

            //string key of the dictionary must be equal the column title in the ui. Value of the dictionary must be a valid column in db/query
            var columnsMap = new Dictionary<string, Expression<Func<StateDetailsViewModel, object>>>()
            {
                ["Code"] = v => v.StateCode,
                ["Name"] = v => v.StateName,
                ["Description"] = v => v.Description,
                ["CountryName"] = v => v.CountryName
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = query.Count();
            //query = query.ApplyPaging(queryObj);

            result.Items = query.ToList();

            return result;
        }

    }
}
