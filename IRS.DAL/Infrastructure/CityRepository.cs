using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using IRS.DAL.Helpers;

namespace IRS.DAL.Infrastructure
{
    public class CityRepository : ICityRepository
    {
        private ApplicationDbContext _context;
        public CityRepository(ApplicationDbContext context)
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

        public bool CheckCityCode(string Code)
        {
            if (_context.Cities.Any(v => v.Code == Code))
                return true;
            else
                return false;
        }

        public async Task<City> GetCity(Guid? id)
        {
            if (id == Guid.Empty)
                return null;
            var city = await _context.Cities
                .Include(x => x.Areas)
                .FirstOrDefaultAsync(x => x.Id == id);

            return city;
        }

        public async Task<IEnumerable<City>> GetCities()
        {
            var cities = await _context.Cities
                .Include(x => x.Areas)
                .ToListAsync();

            return cities;
        }

        public async Task<QueryResult<CityDetailsViewModel>> GetCityList(LocationQuery queryObj)
        {
            var result = new QueryResult<CityDetailsViewModel>();

            var query = (from ct in _context.Cities
                         join st in _context.States
                            on ct.StateId equals st.Id
                            into CityState
                         //do left join
                         from cs in CityState.DefaultIfEmpty()
                         join co in _context.Countries
                            on cs.CountryId equals co.Id
                            into CityStateCountry
                         //do left join
                         from csc in CityStateCountry.DefaultIfEmpty()
                         select new 
                         {
                             Id = ct.Id,
                             CityName = ct.Name,
                             CityCode = ct.Code,
                             Description = ct.Description,
                             StateId = cs.Id,
                             StateName = cs.Name,
                             CountryName = csc.Name,
                             CountryId = csc.Id,
                             Deleted = ct.Deleted
                         })
                         .Where(x => x.Deleted != true)
                         .ToList().Select((s, index) => new CityDetailsViewModel()
                         {
                             SerialNumber = index + 1,
                             Id = s.Id,
                             CityName = s.CityName,
                             CityCode = s.CityCode,
                             Description = s.Description,
                             StateId = s.StateId,
                             StateName = s.StateName,
                             CountryName = s.CountryName,
                             CountryId = s.CountryId
                         })
                         .AsQueryable();

            if (!string.IsNullOrEmpty(queryObj.Name))
                query = query.Where(v => v.CityCode.Contains(queryObj.Name) | v.CityName.Contains(queryObj.Name) | v.Description.Contains(queryObj.Name));

            //string key of the dictionary must be equal the column title in the ui. Value of the dictionary must be a valid column in db/query
            var columnsMap = new Dictionary<string, Expression<Func<CityDetailsViewModel, object>>>()
            {
                ["Code"] = v => v.CityCode,
                ["Name"] = v => v.CityName,
                ["Description"] = v => v.Description,
                ["StateName"] = v => v.StateName,
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
