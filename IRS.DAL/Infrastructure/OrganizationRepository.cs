using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.QueryResources.QueryResult;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using IRS.DAL.Models.SharedResource;
using IRS.DAL.Helpers;
using IRS.DAL.ViewModel;

namespace IRS.DAL.Infrastructure
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private ApplicationDbContext _context;
        public OrganizationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Organization> GetOrganization(Guid? id)
        {
            var org = await _context.Organizations
                .Where(x => x.Deleted != true)
                .FirstOrDefaultAsync(x => x.Id == id);

            return org;
        }

        public async Task<IEnumerable<Organization>> GetOrganizations()
        {
            var orgs = await _context.Organizations
                .Where(x => x.Deleted != true)
                .Include(x => x.OrganizationDepartments)
                .Include(x => x.Users)
                .ToListAsync();

            return orgs;
        }

        public bool CheckOrganizationCode(string Code)
        {
            if (_context.Organizations
                .Where(x => x.Deleted != true)
                .Any(v => v.Code == Code))
                return true;
            else
                return false;
        }

        public async Task<QueryResult<OrganizationViewModel>> GetOrganizationList(OrganizationQuery queryObj)
        {
            var result = new QueryResult<OrganizationViewModel>();

            var query = (from og in _context.Organizations
                         select new
                         {
                             Id = og.Id,
                             Code = og.Code,
                             CompanyName = og.CompanyName,
                             RegistrationNo = og.RegistrationNo,
                             BusinessCategory = og.BusinessCategory,
                             ContactFirstName = og.ContactFirstName,
                             ContactMiddleName = og.ContactMiddleName,
                             ContactLastName = og.ContactLastName,
                             Phone1 = og.Phone1,
                             Phone2 = og.Phone2,
                             OfficeAddress = og.OfficeAddress,
                             BrandLogo = og.BrandLogo,
                             DateofEst = og.DateofEst,
                             Comment = og.Comment,
                             AreaId = og.AreaId,
                             CityId = og.CityId,
                             StateId = og.StateId,
                             CountryId = og.CountryId,
                             Area = og.Area,
                             City = og.City,
                             State = og.State,
                             Country = og.Country,
                             BrandTitle = og.BrandTitle,
                             Email1 = og.Email1,
                             Email2 = og.Email2,
                             UseSsl = og.UseSsl,
                             PageSize = og.PageSize,
                             Port = og.Port,
                             HostName = og.HostName,
                             MaxImageFileSize = og.MaxImageFileSize,
                             AcceptedImageFileTypes = og.AcceptedImageFileTypes,
                             MaxVideoFileSize = og.MaxVideoFileSize,
                             AcceptedVideoFileTypes = og.AcceptedVideoFileTypes,
                             SmsServiceProvider = og.SmsServiceProvider,
                             EmailSenderName = og.EmailSenderName,
                             EmailSendersEmail = og.EmailSendersEmail,
                             EmailSenderPassword = og.EmailSenderPassword,
                             EmailRecipientAddresses = og.EmailRecipientAddresses,
                             SendSMS = og.SendSMS,
                             SMSRecipientNumbers = og.SMSRecipientNumbers,
                             SMSSenderName = og.SMSSenderName,
                             ActivateEmailSenderSettings = og.ActivateEmailSenderSettings,
                             ActivateFileSettings = og.ActivateFileSettings,
                             Deleted = og.Deleted,
                             HazardDefaultDepartmentId = og.HazardDefaultDepartmentId
                         })
                        .Where(x => x.Deleted != true)
                        .ToList().Select((s, index) => new OrganizationViewModel()
                        {
                            SerialNumber = index + 1,
                            Id = s.Id,
                            Code = s.Code,
                            CompanyName = s.CompanyName,
                            RegistrationNo = s.RegistrationNo,
                            BusinessCategory = s.BusinessCategory,
                            ContactFirstName = s.ContactFirstName,
                            ContactMiddleName = s.ContactMiddleName,
                            ContactLastName = s.ContactLastName,
                            Phone1 = s.Phone1,
                            Phone2 = s.Phone2,
                            OfficeAddress = s.OfficeAddress,
                            BrandLogo = s.BrandLogo,
                            DateofEst = s.DateofEst,
                            Comment = s.Comment,
                            AreaId = s.AreaId,
                            CityId = s.CityId,
                            StateId = s.StateId,
                            CountryId = s.CountryId,
                            Area = s.Area,
                            City = s.City,
                            State = s.State,
                            Country = s.Country,
                            BrandTitle = s.BrandTitle,
                            Email1 = s.Email1,
                            Email2 = s.Email2,
                            UseSsl = s.UseSsl,
                            PageSize = s.PageSize,
                            Port = s.Port,
                            HostName = s.HostName,
                            MaxImageFileSize = s.MaxImageFileSize,
                            AcceptedImageFileTypes = s.AcceptedImageFileTypes,
                            MaxVideoFileSize = s.MaxVideoFileSize,
                            AcceptedVideoFileTypes = s.AcceptedVideoFileTypes,
                            SmsServiceProvider = s.SmsServiceProvider,
                            EmailSenderName = s.EmailSenderName,
                            EmailSendersEmail = s.EmailSendersEmail,
                            EmailSenderPassword = s.EmailSenderPassword,
                            EmailRecipientAddresses = s.EmailRecipientAddresses,
                            SendSMS = s.SendSMS,
                            SMSRecipientNumbers = s.SMSRecipientNumbers,
                            SMSSenderName = s.SMSSenderName,
                            ActivateEmailSenderSettings = s.ActivateEmailSenderSettings,
                            ActivateFileSettings = s.ActivateFileSettings,
                            HazardDefaultDepartmentId = s.HazardDefaultDepartmentId
                        })
                        .AsQueryable();

            if (!string.IsNullOrEmpty(queryObj.Name))
                query = query.Where(v => v.Code.Contains(queryObj.Name) | v.CompanyName.Contains(queryObj.Name) | v.OfficeAddress.Contains(queryObj.Name));

            //string key of the dictionary must be equal the column title in the ui. Value of the dictionary must be a valid column in db/query
            var columnsMap = new Dictionary<string, Expression<Func<OrganizationViewModel, object>>>()
            {
                ["Code"] = v => v.Code,
                ["CompanyName"] = v => v.CompanyName,
                ["BusinessCategory"] = v => v.BusinessCategory,
                ["OfficeAddress"] = v => v.OfficeAddress,
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObj);

            result.Items = query.ToList();

            return result;
        }
    }
}
