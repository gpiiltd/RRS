using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IRS.DAL.Helpers;
using System.Linq;
using IRS.DAL.Models.QueryResource.Incidence;

namespace IRS.DAL.Infrastructure
{
    public class IncidenceTypeDepartmentRepository : IIncidenceTypeDepartmentRepository
    {
        private ApplicationDbContext _context;
        private IDepartmentRepository _deptRepo;
        private readonly IUserRepository _userRepo;
        public IncidenceTypeDepartmentRepository(ApplicationDbContext context, IDepartmentRepository deptRepo, IUserRepository userRepo)
        {
            _context = context;
            _deptRepo = deptRepo;
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

        public async Task<IEnumerable<IncidenceType>> GetUnmappedIncidenceTypes()
        {
            var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
            var query = _context.IncidenceTypes
                .Where(x => !_context.IncidenceTypeDepartmentMappings.Select(y => y.IncidenceTypeId)
                      .Contains(x.Id) && x.Deleted != true).AsQueryable();

            if (user != null && user.UserName != "Admin")
                query = query.Where(x => x.OrganizationId == user.OrganizationId);

            return await query.ToListAsync();
        }

        public bool CheckIncidenceTypeDepartment(Guid? IncidenceTypeId, Guid? OrganizationId)
        {
            if (_context.IncidenceTypeDepartmentMappings.Any(v => v.IncidenceTypeId == IncidenceTypeId && v.OrganizationId == OrganizationId))
                return true;
            else
                return false;
        }

        public async Task<IncidenceTypeDepartmentMapping> GetIncidenceTypeDepartment(Guid? id)
        {
            if (id != null)
            {
                var incidenceTypeDept = await _context.IncidenceTypeDepartmentMappings
                .FirstOrDefaultAsync(x => x.Id == id);

                return incidenceTypeDept;
            }
            return null;
        }

        public async Task<OrganizationDepartment> GetDepartmentFromIncidenceType(Guid? incidenceTypeId)
        {
            //to do : test if it works
            if (incidenceTypeId != null)
            {
                var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
                var query = _context.IncidenceTypeDepartmentMappings
                                        .Where(x => x.IncidenceTypeId == incidenceTypeId).AsQueryable();

                if (user != null && user.UserName != "Admin")
                    query = query.Where(x => x.OrganizationId == user.OrganizationId);

                var incidenceTypeDept = await query.FirstOrDefaultAsync(x => x.IncidenceTypeId == incidenceTypeId);
                if (incidenceTypeDept.DepartmentId != null && user != null)
                {
                    var dept = await _deptRepo.GetDepartment(incidenceTypeDept.DepartmentId);
                    return dept;
                }
            }
            return null;
        }

        public async Task<QueryResult<IncidenceTypeDepartmentViewModel>> GetIncidenceTypesDepartmentsList(IncidenceTypeDepartmentQuery queryObj)
        {
            var result = new QueryResult<IncidenceTypeDepartmentViewModel>();
            var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());

            var query = (from it in _context.IncidenceTypeDepartmentMappings
                         select new 
                         {
                             Id = it.Id,
                             DepartmentId = it.DepartmentId,
                             DepartmentName = it.Department.Name,
                             IncidenceTypeId = it.IncidenceTypeId,
                             IncidenceTypeName = it.IncidenceType.Name,
                             OrganizationId = it.OrganizationId,
                             OrganizationName = it.Organization.CompanyName
                         })
                         .Where(x => user.UserName != "Admin" ? x.OrganizationId == user.OrganizationId : false)
                         .ToList().Select((s, index) => new IncidenceTypeDepartmentViewModel()
                         {
                             SerialNumber = index + 1,
                             Id = s.Id,
                             DepartmentId = s.DepartmentId,
                             DepartmentName = s.DepartmentName,
                             IncidenceTypeId = s.IncidenceTypeId,
                             IncidenceTypeName = s.IncidenceTypeName,
                             OrganizationId = s.OrganizationId,
                             OrganizationName = s.OrganizationName
                         })
                         .AsQueryable();

            if (!string.IsNullOrEmpty(queryObj.Name))
                query = query.Where(v => v.DepartmentName.Contains(queryObj.Name) | v.DepartmentName.Contains(queryObj.Name));

            //string key of the dictionary must be equal the column title in the ui. Value of the dictionary must be a valid column in db/query
            var columnsMap = new Dictionary<string, Expression<Func<IncidenceTypeDepartmentViewModel, object>>>()
            {
                ["DepartmentName"] = v => v.DepartmentName,
                ["IncidenceTypeName"] = v => v.IncidenceTypeName
            };

            query = query.ApplyOrdering(queryObj, columnsMap);

            result.TotalItems = query.Count();
            query = query.ApplyPaging(queryObj);

            result.Items = query.ToList();

            return result;
        }

    }
}
