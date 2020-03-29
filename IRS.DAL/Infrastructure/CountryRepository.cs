using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.QueryResources.QueryResult;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using IRS.DAL.Helpers;

namespace IRS.DAL.Infrastructure
{
    public class CountryRepository : ICountryRepository
    {
        private ApplicationDbContext _context;
        public CountryRepository(ApplicationDbContext context)
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

        public async Task<Country> GetCountry(Guid? id)
        {
            var country = await _context.Countries
                .Include(x => x.States)
                    .ThenInclude(x => x.Cities)
                .Include(x => x.States)
                    .ThenInclude(x => x.Areas)
                .FirstOrDefaultAsync(x => x.Id == id);

            return country;
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            var countries = await _context.Countries
                .Include(x => x.States)
                    .ThenInclude(x => x.Cities)
                .Include(x => x.States)
                    .ThenInclude(x => x.Areas)
                .ToListAsync();

            return countries;
        }

        public bool CheckCountryCode(string Code1, string Code2)
        {
            if (_context.Countries.Any(v => v.Code1 == Code1))
                return true;
            else if (_context.Countries.Any(v => v.Code2 == Code2))
                return true;
            else
                return false;
        }

        public async Task<QueryResult<CountryDetailsDto>> GetCountryList(LocationQuery queryObj)
        {
            var result = new QueryResult<CountryDetailsDto>();

            var query = (from co in _context.Countries
                         select new 
                         {
                             Id = co.Id,
                             Code1 = co.Code1,
                             Code2 = co.Code2,
                             Description = co.Description,
                             Name = co.Name,
                             Deleted = co.Deleted
                         })
                         .Where(x => x.Deleted != true)
                         .ToList().Select((s, index) => new CountryDetailsDto()
                         {
                             SerialNumber = index + 1,
                             Id = s.Id,
                             Code1 = s.Code1,
                             Code2 = s.Code2,
                             Description = s.Description,
                             Name = s.Name,
                         })
                         .AsQueryable();

            if (!string.IsNullOrEmpty(queryObj.Name))
                query = query.Where(v => v.Code1.Contains(queryObj.Name) | v.Code2.Contains(queryObj.Name) | v.Name.Contains(queryObj.Name) | v.Description.Contains(queryObj.Name));

            //string key of the dictionary must be equal the column title in the ui. Value of the dictionary must be a valid column in db/query
            var columnsMap = new Dictionary<string, Expression<Func<CountryDetailsDto, object>>>()
            {
                ["Code1"] = v => v.Code1,
                ["Code2"] = v => v.Code2,
                ["Name"] = v => v.Name,
                ["Description"] = v => v.Description,
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = query.Count();
            // query = query.ApplyPaging(queryObj);

            result.Items = query.ToList();

            return result;
        }
    }
}
