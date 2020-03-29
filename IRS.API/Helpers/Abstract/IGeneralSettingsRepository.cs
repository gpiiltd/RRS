using IRS.API.Dtos;
using IRS.API.Helpers;
using IRS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Helpers.Abstract
{
    public interface IGeneralSettingsRepository
    {
        void Add<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        Task<bool> CheckSystemSettings();
        Task<SystemSetting> GetSystemSettingsModel();
        Task<SystemSettingsDto> GetSystemSettings();
    }
}
