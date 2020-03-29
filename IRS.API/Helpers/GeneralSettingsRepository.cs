using IRS.API.Dtos;
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
    public class GeneralSettingsRepository : IGeneralSettingsRepository
    {
        //repository: collection of domain objects in memory
        private readonly ApplicationDbContext _context;

        public GeneralSettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> CheckSystemSettings()
        {
            if (_context.SystemSettings.Any())
                return true;
            else
                return false;
        }

        public async Task<SystemSetting> GetSystemSettingsModel()
        {
            return await _context.SystemSettings
              .FirstOrDefaultAsync();
        }

        public async Task<SystemSettingsDto> GetSystemSettings()
        {
            var query = (from org in _context.SystemSettings
                         select new
                         {
                             Id = org.Id,
                             Email1 = org.Email1,
                             Email2 = org.Email2,
                             UseSsl = org.UseSsl,
                             HostName = org.HostName,
                             Port = org.Port,
                             MaxImageFileSize = org.MaxImageFileSize,
                             AcceptedImageFileTypes = org.AcceptedImageFileTypes, 
                             MaxVideoFileSize = org.MaxVideoFileSize,
                             AcceptedVideoFileTypes = org.AcceptedVideoFileTypes,
                             PageSize = org.PageSize,
                             SmsServiceProvider = org.SmsServiceProvider,
                             EmailSendersEmail = org.EmailSendersEmail,
                             EmailSenderName = org.EmailSenderName,
                             EmailSenderPassword = org.EmailSenderPassword,
                             SendSMS = org.SendSMS,
                             OrganizationName = org.EmailSenderName
                         }).ToList().Select((s) => new SystemSettingsDto()
                         {
                             Id = s.Id,
                             Email1 = s.Email1,
                             Email2 = s.Email2,
                             UseSsl = s.UseSsl,
                             HostName = s.HostName,
                             Port = s.Port,
                             MaxImageFileSize = s.MaxImageFileSize,
                             MaxVideoFileSize = s.MaxVideoFileSize,
                             AcceptedImageFileTypes = s.AcceptedImageFileTypes,
                             AcceptedVideoFileTypes = s.AcceptedVideoFileTypes,
                             PageSize = s.PageSize,
                             SmsServiceProvider = s.SmsServiceProvider,
                             EmailSendersEmail = s.EmailSendersEmail,
                             EmailSenderName = s.EmailSenderName,
                             EmailSenderPassword = s.EmailSenderPassword,
                             SendSMS = s.SendSMS,
                             OrganizationName = s.OrganizationName
                         }).FirstOrDefault();

            return query;

        }
    }
}
