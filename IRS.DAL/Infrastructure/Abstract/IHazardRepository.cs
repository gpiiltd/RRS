using IRS.DAL.Models;
using IRS.DAL.Models.QueryResource.Hazard;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DAL.Infrastructure.Abstract
{
    public interface IHazardRepository
    {
        Task<Hazard> GetHazard(Guid id, bool includeRelated = true);
        void Add(Hazard incidence);
        void Remove(Hazard incidence);
        bool CheckHazardCode(string Code);
        Task<QueryResult<HazardViewModel>> GetHazards(HazardQuery queryObj);
    }
}
