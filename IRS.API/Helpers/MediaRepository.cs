using IRS.API.Helpers.Abstract;
using IRS.DAL;
using IRS.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IRS.API.Helpers
{
    public class MediaRepository : IMediaRepository
    {
        private readonly ApplicationDbContext context;

        public MediaRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Media>> GetPhotos(Guid incidenceId)
        {
            var photos = await context.Gallery
                .Where(x => x.IncidenceId == incidenceId)
                .ToListAsync();

            return photos;
        }

        public async Task<Media> GetPhoto(Guid id)
        {
            var photos = await context.Gallery
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return photos;
        }

        public async Task<IEnumerable<Media>> GetHazardPhotos(Guid hazardId)
        {
            var photos = await context.Gallery
                .Where(x => x.HazardId == hazardId)
                .ToListAsync();

            return photos;
        }

        public async Task<Media> GetUserProfilePhoto(Guid userId)
        {
            var photos = await context.Gallery
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            return photos;
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }
    }
}
