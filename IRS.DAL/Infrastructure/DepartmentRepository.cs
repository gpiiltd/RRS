using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.QueryResources.QueryResult;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using IRS.DAL.Models.SharedResource;
using IRS.DAL.Helpers;
using IRS.DAL.ViewModel;

namespace IRS.DAL.Infrastructure
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private ApplicationDbContext _context;
        private readonly IUserRepository _userRepo;
        public DepartmentRepository(ApplicationDbContext context, IUserRepository userRepo)
        {
            _context = context;
            _userRepo = userRepo;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<OrganizationDepartment> GetDepartment(Guid? id)
        {
            if (id != null)
            {
                var dept = await _context.OrganizationDepartments
                .FirstOrDefaultAsync(x => x.Id == id);

                return dept;
            }
            return null;
        }

        public async Task<IEnumerable<OrganizationDepartment>> GetDepartments()
        {
            var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
            var query = _context.OrganizationDepartments
                .Where(x => x.Deleted != true)
                .AsQueryable();

            if (user != null && user.UserName != "Admin")
                query = query.Where(x => x.OrganizationId == user.OrganizationId)
                    .Include(x => x.Users);

            return await query.ToListAsync();
        }

        public bool CheckDepartmentCode(string Code, Guid? OrganizationId)
        {
            if (_context.OrganizationDepartments
                .Where(x => x.Deleted != true)
                .Any(v => v.Code == Code && v.OrganizationId == OrganizationId))
                return true;
            else
                return false;
        }

        public async Task<QueryResult<DepartmentViewModel>> GetDepartmentList(DepartmentQuery queryObj)
        {
            var result = new QueryResult<DepartmentViewModel>();
            var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());

            var query = (from d in _context.OrganizationDepartments
                             //join og in _context.Organizations
                             //   on d.OrganizationId equals og.Id into OrgDepts
                             ////do left join
                             //from od in OrgDepts.DefaultIfEmpty()
                         select new
                         {
                             Id = d.Id,
                             Code = d.Code,
                             Name = d.Name,
                             Description = d.Description,
                             OrganizationId = d.OrganizationId,
                             Organization = d.Organization,
                             Email1 = d.Email1,
                             Email2 = d.Email2,
                             Phone1 = d.Phone1,
                             Phone2 = d.Phone2,
                             UseSsl = d.UseSsl,
                             HostName = d.HostName,
                             Port = d.Port,
                             MaxImageFileSize = d.MaxImageFileSize,
                             AcceptedImageFileTypes = d.AcceptedImageFileTypes,
                             MaxVideoFileSize = d.MaxVideoFileSize,
                             AcceptedVideoFileTypes = d.AcceptedVideoFileTypes,
                             SmsServiceProvider = d.SmsServiceProvider,
                             EmailSenderName = d.EmailSenderName,
                             EmailSendersEmail = d.EmailSendersEmail,
                             EmailSenderPassword = d.EmailSenderPassword,
                             EmailRecipientAddresses = d.EmailRecipientAddresses,
                             SendSMS = d.SendSMS,
                             SMSRecipientNumbers = d.SMSRecipientNumbers,
                             SMSSenderName = d.SMSSenderName,
                             ActivateEmailSenderSettings = d.ActivateEmailSenderSettings,
                             ActivateFileSettings = d.ActivateFileSettings,
                             Deleted = d.Deleted
                             //DateCreated = d.DateCreated,
                             //CreatedByUserId = inc.CreatedByUserId,
                             //DateEdited = inc.DateEdited,
                             //EditedByUserId = inc.EditedByUserId,
                         })
                         .Where(x => user.UserName != "Admin" ? x.OrganizationId == user.OrganizationId && x.Deleted != true : x.Deleted != true)
                         .ToList().Select((s, index) => new DepartmentViewModel()
                         {
                             SerialNumber = index + 1,
                             Id = s.Id,
                             Code = s.Code,
                             Name = s.Name,
                             Description = s.Description,
                             OrganizationId = s.OrganizationId,
                             Email1 = s.Email1,
                             Email2 = s.Email2,
                             Phone1 = s.Phone1,
                             Phone2 = s.Phone2,
                             UseSsl = s.UseSsl,
                             HostName = s.HostName,
                             Port = s.Port,
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
                             ActivateFileSettings = s.ActivateFileSettings
                         })
                         .AsQueryable();

            if (!string.IsNullOrEmpty(queryObj.Name))
                query = query.Where(v => v.Code.Contains(queryObj.Code) | v.Name.Contains(queryObj.Name) | v.Description.Contains(queryObj.Description));

            //string key of the dictionary must be equal the column title in the ui. Value of the dictionary must be a valid column in db/query
            var columnsMap = new Dictionary<string, Expression<Func<DepartmentViewModel, object>>>()
            {
                ["Code"] = v => v.Code,
                ["Name"] = v => v.Name,
                ["Descriotion"] = v => v.Description
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = query.Count();
            // query = query.ApplyPaging(queryObj);

            result.Items = query.ToList();

            return result;
        }
    }
}
