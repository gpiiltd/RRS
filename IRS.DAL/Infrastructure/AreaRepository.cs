using IRS.DAL.Helpers;
using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IRS.DAL.Infrastructure
{
    public class AreaRepository : IAreaRepository
    {
        private ApplicationDbContext _context;
        private readonly IUserRepository _userRepo;
        public AreaRepository(ApplicationDbContext context, IUserRepository userRepo)
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
        public async Task<Area> GetArea(Guid id)
        {
            var area = await _context.Areas
                .FirstOrDefaultAsync(x => x.Id == id);

            return area;
        }

        public bool CheckAreaCode(string Code)
        {
            if (_context.Areas.Any(v => v.Code == Code))
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<Area>> GetAreas()
        {
            var areas = await _context.Areas
                .ToListAsync();

            return areas;
        }

        public async Task<QueryResult<AreaDetailsViewModel>> GetAreaList(LocationQuery queryObj)
        {
            var result = new QueryResult<AreaDetailsViewModel>();

            var query = (from ar in _context.Areas
                         join ct in _context.Cities
                            on ar.CityId equals ct.Id
                            into AreaCities
                         //do left-join
                         from ac in AreaCities.DefaultIfEmpty()
                         join st in _context.States
                            on ar.StateId equals st.Id
                            into AreaCitiesStates
                         //do left join
                         from acs in AreaCitiesStates.DefaultIfEmpty()
                         join co in _context.Countries
                            on acs.CountryId equals co.Id
                            into AreaCitiesStatesCountry
                         //do left join
                         from acsc in AreaCitiesStatesCountry.DefaultIfEmpty()
                         select new 
                            {
                                Id = ar.Id,
                                AreaName = ar.Name,
                                AreaCode = ar.Code,
                                Description = ar.Description,
                                CityName = ac.Name,
                                CityId = ar.CityId,
                                StateName = acs.Name,
                                StateId = ar.StateId,
                                CountryName = acsc.Name,
                                CountryId = acsc.Id,
                                Deleted = ar.Deleted
                             })
                         .Where(x => x.Deleted != true)
                         .ToList().Select((s, index) => new AreaDetailsViewModel()
                         {
                             SerialNumber = index + 1,
                             Id = s.Id,
                             AreaName = s.AreaName,
                             AreaCode = s.AreaCode,
                             Description = s.Description,
                             CityName = s.CityName,
                             CityId = s.CityId,
                             StateName = s.StateName,
                             StateId = s.StateId,
                             CountryName = s.CountryName,
                             CountryId = s.CountryId
                         })
                         .AsQueryable();

            if (!string.IsNullOrEmpty(queryObj.Name))
                query = query.Where(v => v.AreaCode.Contains(queryObj.Name) | v.AreaName.Contains(queryObj.Name) | v.Description.Contains(queryObj.Name));

            //string key of the dictionary must be equal the column title in the ui. Value of the dictionary must be a valid column in db/query
            var columnsMap = new Dictionary<string, Expression<Func<AreaDetailsViewModel, object>>>()
            {
                ["Code"] = v => v.AreaCode,
                ["Name"] = v => v.AreaName,
                ["Description"] = v => v.Description,
                ["CityName"] = v => v.CityName,
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
