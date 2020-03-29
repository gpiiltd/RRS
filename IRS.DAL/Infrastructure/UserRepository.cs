using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using IRS.DAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using IRS.DAL.Models.QueryResources.Users;
using IRS.DAL.Models.QueryResources.QueryResult;
using System.Linq.Expressions;
using IRS.DAL.Helpers;
using IRS.DAL.Models.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IRS.DAL.Infrastructure
{
    public class UserRepository: IUserRepository
    {
        private ApplicationDbContext _context;
        private IHttpContextAccessor _contextAccessor;
        public UserRepository(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var user = await GetUser(GetLoggedInUserId());
            var users = await _context.Users
                .Where(x => user.UserName != "Admin" ? x.OrganizationId == user.OrganizationId && x.Deleted != true : x.Deleted != true)
                .ToListAsync();

            return users;
        }

        public async Task<User> GetUser(Guid? id)
        {
            if (id != null)
            {
                var user = await _context.Users
                //.Include(x => x.AssignedIncidences)
                //    .ThenInclude(y => y.IncidenceTypes)
                //.Include(x => x.AssignedIncidences)
                //    .ThenInclude(y => y.IncidenceStatuses)
                //.Include(x => x.AllocatedIncidences)
                //    .ThenInclude(y => y.IncidenceTypes)
                //.Include(x => x.AllocatedIncidences)
                //    .ThenInclude(y => y.IncidenceStatuses)
                .FirstOrDefaultAsync(x => x.Id == id);

                return user;
            }

            return null;
        }

        public async Task<UserDeployment> GetUserDeploymentData(Guid? id)
        {
            if (id != null)
            {
                var userDeploy = await _context.UserDeployment
                .FirstOrDefaultAsync(x => x.UserId == id);

                return userDeploy;
            }

            return null;
        }

        public async Task<UserDetailsViewModel> GetUserFullData(Guid id)
        {
            var userDetails = (from us in _context.Users
                               join ud in _context.UserDeployment
                                   on us.Id equals ud.UserId
                                   into UserDeploy
                               //do left-join
                               from usd in UserDeploy.DefaultIfEmpty()
                               join od in _context.OrganizationDepartments
                                   on usd.DepartmentId equals od.Id
                                       into UserDeploymentAndDept
                               //do left-join
                               from udd in UserDeploymentAndDept.DefaultIfEmpty()
                               join o in _context.Organizations
                                   on udd.OrganizationId equals o.Id
                                   into UserDeploymentOrgDept
                               //do left-join
                               from uod in UserDeploymentOrgDept.DefaultIfEmpty()
                               join g in _context.Gallery
                                   on us.Id equals g.UserId
                                   into UserGalleryAndDeploymentOrgDept
                               //do left-join
                               from ug in UserGalleryAndDeploymentOrgDept.DefaultIfEmpty()
                               where us.Id == id
                               select new UserDetailsViewModel
                               {
                                   Id = us.Id, 
                                   Username = us.UserName,
                                   FirstName = us.FirstName,
                                   MiddleName = us.MiddleName,
                                   LastName = us.LastName,
                                   JobTitle = us.JobTitle,
                                   Email1 = us.Email1,
                                   Email2 = us.Email2,
                                   Phone1 = us.Phone1,
                                   Phone2 = us.Phone2,
                                   StaffNo = us.StaffNo,
                                   DateOfBirth = us.DateOfBirth,
                                   KnownAs = us.KnownAs,
                                   LastActive = us.LastActive,
                                   Introduction = us.Introduction,
                                   AreaOfOriginId = us.AreaOfOriginId,
                                   CityOfOriginId = us.CityOfOriginId,
                                   StateOfOriginId = us.StateOfOriginId,
                                   CountryOfOriginId = us.CountryOfOriginId,
                                   DepartmentId = usd.DepartmentId,
                                   OrganizationId = udd.OrganizationId,
                                   UserDepartment = usd.UserDepartment,
                                   UserOrganization = udd.Organization,
                                   Gender = us.Gender,
                                   PreferredContactMethod = us.PreferredContactMethod,
                                   ProfilePhotoName = ug.FileName,
                                   AreaOfDeploymentId = usd.AreaOfDeploymentId,
                                   CityOfDeploymentId = usd.CityOfDeploymentId,
                                   StateOfDeploymentId = usd.StateOfDeploymentId,
                                   CountryOfDeploymentId = usd.CountryOfDeploymentId,
                                   AreaOfDeployment = usd.AreaOfDeployment,
                                   CityOfDeployment = usd.CityOfDeployment,
                                   StateOfDeployment = usd.StateOfDeployment,
                                   CountryOfDeployment = usd.CountryOfDeployment,
                                   DateOfDeployment = usd.DateOfDeployment,
                                   DateOfSignOff = usd.DateOfSignOff,
                                   IsActive = us.IsActive,
                                   MobileAppLoginPattern = us.MobileAppLoginPattern
                               }).FirstOrDefaultAsync();

            return await userDetails;
        }

        public async Task<QueryResult<UserDetailsViewModel>> GetUserList(UsersQuery queryObj)
        {
            var user = await GetUser(GetLoggedInUserId());
            var query = (from us in _context.Users
                         join ud in _context.UserDeployment
                             on us.Id equals ud.UserId
                             into UserDeploy
                         //do left-join
                         from usd in UserDeploy.DefaultIfEmpty()
                         join g in _context.Gallery
                                   on us.Id equals g.UserId
                                   into UserGalleryDeploy
                         //do left-join
                         from ug in UserGalleryDeploy.DefaultIfEmpty()
                         select new 
                               {
                                    Id = us.Id,
                                    Username = us.UserName,
                                    FirstName = us.FirstName,
                                    MiddleName = us.MiddleName,
                                    LastName = us.LastName,
                                    JobTitle = us.JobTitle,
                                    Email1 = us.Email1,
                                    Email2 = us.Email2,
                                    Phone1 = us.Phone1,
                                    Phone2 = us.Phone2,
                                    StaffNo = us.StaffNo,
                                    DateOfBirth = us.DateOfBirth,
                                    KnownAs = us.KnownAs,
                                    LastActive = us.LastActive,
                                    Introduction = us.Introduction,
                                    AreaOfOriginId = us.AreaOfOriginId,
                                    CityOfOriginId = us.CityOfOriginId,
                                    StateOfOriginId = us.StateOfOriginId,
                                    CountryOfOriginId = us.CountryOfOriginId,
                                    DepartmentId = usd.DepartmentId,
                                    OrganizationId = us.OrganizationId,
                                    UserDepartment = usd.UserDepartment,
                                    UserOrganization = us.UserOrganization,
                                    Gender = us.Gender,
                                    PreferredContactMethod = us.PreferredContactMethod,
                                    ProfilePhotoName = ug.FileName,
                                    AreaOfDeploymentId = usd.AreaOfDeploymentId,
                                    CityOfDeploymentId = usd.CityOfDeploymentId,
                                    StateOfDeploymentId = usd.StateOfDeploymentId,
                                    CountryOfDeploymentId = usd.CountryOfDeploymentId,
                                    AreaOfDeployment = usd.AreaOfDeployment,
                                    CityOfDeployment = usd.CityOfDeployment,
                                    StateOfDeployment = usd.StateOfDeployment,
                                    CountryOfDeployment = usd.CountryOfDeployment,
                                    DateOfDeployment = usd.DateOfDeployment,
                                    DateOfSignOff = usd.DateOfSignOff,
                                    Deleted = us.Deleted
                         }).Where(x => user.UserName != "Admin" ? x.OrganizationId == user.OrganizationId && x.Username != "Admin" && x.Deleted != true : x.Deleted != true)
                         .ToList().Select((s, index) => new UserDetailsViewModel()
                               {
                                    SerialNumber = index + 1,
                                    Id = s.Id,
                                    Username = s.Username,
                                    FirstName = s.FirstName,
                                    MiddleName = s.MiddleName,
                                    LastName = s.LastName,
                                    JobTitle = s.JobTitle,
                                    Email1 = s.Email1,
                                    Email2 = s.Email2,
                                    Phone1 = s.Phone1,
                                    Phone2 = s.Phone2,
                                    StaffNo = s.StaffNo,
                                    DateOfBirth = s.DateOfBirth,
                                    KnownAs = s.KnownAs,
                                    LastActive = s.LastActive,
                                    Introduction = s.Introduction,
                                    AreaOfOriginId = s.AreaOfOriginId,
                                    CityOfOriginId = s.CityOfOriginId,
                                    StateOfOriginId = s.StateOfOriginId,
                                    CountryOfOriginId = s.CountryOfOriginId,
                                    DepartmentId = s.DepartmentId,
                                    OrganizationId = s.OrganizationId,
                                    UserDepartment = s.UserDepartment,
                                    UserOrganization = s.UserOrganization,
                                    Gender = s.Gender,
                                    PreferredContactMethod = s.PreferredContactMethod,
                                    ProfilePhotoName = s.ProfilePhotoName,
                                    AreaOfDeploymentId = s.AreaOfDeploymentId,
                                    CityOfDeploymentId = s.CityOfDeploymentId,
                                    StateOfDeploymentId = s.StateOfDeploymentId,
                                    CountryOfDeploymentId = s.CountryOfDeploymentId,
                                    AreaOfDeployment = s.AreaOfDeployment,
                                    CityOfDeployment = s.CityOfDeployment,
                                    StateOfDeployment = s.StateOfDeployment,
                                    CountryOfDeployment = s.CountryOfDeployment,
                                    DateOfDeployment = s.DateOfDeployment,
                                    DateOfSignOff = s.DateOfSignOff
                         })
                         .AsQueryable();

            var result = new QueryResult<UserDetailsViewModel>();

            if (!string.IsNullOrEmpty(queryObj.FullName))
                query = query.Where(v => v.FirstName.Contains(queryObj.FullName) | v.MiddleName.Contains(queryObj.FullName) | v.LastName.Contains(queryObj.FullName));

            //string key of the dictionary must be equal the column title in the ui. Value of the dictionary must be a valid column in db/query
            var columnsMap = new Dictionary<string, Expression<Func<UserDetailsViewModel, object>>>()
            {
                ["firstName"] = v => v.FirstName,
                ["alias"] = v => v.KnownAs,
                ["username"] = v => v.Username,
                ["gender"] = v => v.Gender,
                ["email1"] = v => v.Email1
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = query.Count();
            //query = query.ApplyPaging(queryObj);

            result.Items = query.ToList();

            return result;
        }

        public async Task<QueryResult<UsersRolesViewModel>> GetUsersWithRoles(UsersRolesQuery queryObj)
        {
            var result = new QueryResult<UsersRolesViewModel>();
            var user = await GetUser(GetLoggedInUserId());
            var query = (from us in _context.Users
                         join ud in _context.UserDeployment
                                   on us.Id equals ud.UserId
                                       into UserDeploy
                         //do left-join
                         from usd in UserDeploy.DefaultIfEmpty()
                         //join o in _context.Organizations
                         //    on usd.OrganizationId equals o.Id
                         //    into UserDeptOrg
                         ////do left-join
                         //from udo in UserDeptOrg.DefaultIfEmpty()
                         select new 
                         {
                             Id = us.Id,
                             UserName = us.UserName,
                             FirstName = us.FirstName,
                             MiddleName = us.MiddleName,
                             LastName = us.LastName,
                             JobTitle = us.JobTitle,
                             StaffNo = us.StaffNo,
                             UserDepartment = usd.UserDepartment,
                             UserOrganization = us.UserOrganization,
                             OrganizationId = us.OrganizationId,
                             Deleted = us.Deleted,
                             Roles = (from userRole in us.UserRoles
                                      join role in _context.Roles
                                      on userRole.RoleId equals role.Id
                                      select role.Name).ToList(),
                             AvailableRoles = (from allRoles in _context.Roles
                                               select allRoles.Name).ToList()
                             
                        })
                        .Where(x => user.UserName != "Admin" ? x.OrganizationId == user.OrganizationId && x.UserName != "Admin" && x.Deleted != true : x.Deleted != true)
                         .ToList().Select((s, index) => new UsersRolesViewModel()
                         {
                             SerialNumber = index + 1,
                             Id = s.Id,
                             UserName = s.UserName,
                             FirstName = s.FirstName,
                             MiddleName = s.MiddleName,
                             LastName = s.LastName,
                             JobTitle = s.JobTitle,
                             StaffNo = s.StaffNo,
                             UserDepartment = s.UserDepartment,
                             UserOrganization = s.UserOrganization,
                             OrganizationId = s.OrganizationId,
                             RolesForUser = s.Roles,
                             UserRoleString = string.Join(", ", s.Roles.ToArray()),
                             AvailableRoles = s.AvailableRoles,
                             AvailableRolesString = string.Join(", ", s.AvailableRoles.ToArray()),
                         })
                         .AsQueryable();

            if (!string.IsNullOrEmpty(queryObj.Name))
                query = query.Where(v => v.FirstName.Contains(queryObj.Name, StringComparison.OrdinalIgnoreCase) | v.MiddleName.Contains(queryObj.Name, StringComparison.OrdinalIgnoreCase) 
                | v.LastName.Contains(queryObj.Name, StringComparison.OrdinalIgnoreCase) | v.StaffNo.Contains(queryObj.Name, StringComparison.OrdinalIgnoreCase) 
                | v.RolesForUser.Any(x => x.Contains(queryObj.Name, StringComparison.OrdinalIgnoreCase)));

            //string key of the dictionary must be equal the column title in the ui. Value of the dictionary must be a valid column in db/query
            var columnsMap = new Dictionary<string, Expression<Func<UsersRolesViewModel, object>>>()
            {
                ["firstName"] = v => v.FirstName,
                ["alias"] = v => v.KnownAs,
                ["username"] = v => v.UserName,
                ["gender"] = v => v.Gender,
                ["email1"] = v => v.Email1,
                ["Roles"] = v => v.RolesForUser,
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObj);

            result.Items = query.ToList();

            return result;

        }

        public Guid GetLoggedInUserId()
        {
            return LoggedInUserID;
        }

        public Guid LoggedInUserID
        {
            get
            {
                var userID = Guid.Empty;
                Guid.TryParse(_contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userID);
                return userID;
            }
        }

        public bool CheckUser(string Username)
        {
            if (_context.Users.Any(v => v.UserName == Username))
                return true;
            else
                return false;
        }
    }
}
