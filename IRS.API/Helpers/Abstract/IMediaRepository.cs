using IRS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Helpers.Abstract
{
    public interface IMediaRepository
    {
        Task<IEnumerable<Media>> GetPhotos(Guid incidenceId);
        Task<IEnumerable<Media>> GetHazardPhotos(Guid hazardId);
        Task<Media> GetUserProfilePhoto(Guid userId);
        Task<Media> GetPhoto(Guid id);
        void Delete<T>(T entity) where T : class;
    }
}
