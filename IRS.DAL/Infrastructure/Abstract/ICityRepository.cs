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
    public interface ICityRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        bool CheckCityCode(string Code);
        Task<City> GetCity(Guid? id);
        Task<IEnumerable<City>> GetCities();
        Task<QueryResult<CityDetailsViewModel>> GetCityList(LocationQuery queryObj);
    }
}
