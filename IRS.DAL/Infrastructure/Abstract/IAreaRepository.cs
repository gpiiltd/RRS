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
    public interface IAreaRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        bool CheckAreaCode(string Code);
        Task<Area> GetArea(Guid id);
        Task<IEnumerable<Area>> GetAreas();
        //Task<QueryResult<Area>> GetAreaList(AreasQuery queryObj);
        Task<QueryResult<AreaDetailsViewModel>> GetAreaList(LocationQuery queryObj);
    }
}
