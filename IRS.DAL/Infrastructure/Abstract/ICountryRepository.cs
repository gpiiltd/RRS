using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IRS.DAL.Models;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.QueryResources.QueryResult;

namespace IRS.DAL.Infrastructure.Abstract
{
    public interface ICountryRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        bool CheckCountryCode(string Code1, string Code2);
        Task<Country> GetCountry(Guid? id);
        Task<IEnumerable<Country>> GetCountries();
        Task<QueryResult<CountryDetailsDto>> GetCountryList(LocationQuery queryObj);
    }
}
