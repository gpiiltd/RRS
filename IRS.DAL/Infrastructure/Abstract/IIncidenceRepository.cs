using IRS.DAL.Models;
using IRS.DAL.Models.QueryResource.Incidence;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DAL.Infrastructure.Abstract
{
    public interface IIncidenceRepository
    {
        Task<Incidence> GetIncidence(Guid id, bool includeRelated = true);
        void Add(Incidence incidence);
        void Remove(Incidence incidence);
        bool CheckIncidenceCode(string Code);
        Task<QueryResult<IncidenceViewModel>> GetIncidences(IncidenceQuery queryObj);
    }
}
