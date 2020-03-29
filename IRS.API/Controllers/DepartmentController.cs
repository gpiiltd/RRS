using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IRS.DAL.Infrastructure.Abstract;
using Microsoft.AspNetCore.Mvc;
using IRS.API.Dtos.UserResources;
using Microsoft.AspNetCore.Authorization;
using IRS.DAL.Models.SharedResource;
using IRS.API.Dtos.LocationDto;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.ViewModel;
using IRS.DAL.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRS.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class DepartmentController : ControllerBase
    {
        private IDepartmentRepository _deptRepo;
        private IOrganizationRepository _orgRepo;
        public IMapper _mapper;
        private readonly IUnitofWork _unitOfWork;
        private readonly IUserRepository _userRepo;

        public DepartmentController(IDepartmentRepository deptRepo, IMapper mapper, IOrganizationRepository orgRepo, IUnitofWork unitOfWork, IUserRepository userRepo)
        {
            _deptRepo = deptRepo;
            _mapper = mapper;
            _orgRepo = orgRepo;
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
        }

        [HttpGet]
        [Route("getDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            var deptFromRepo = await _deptRepo.GetDepartments();
            var deptListDto = _mapper.Map<IEnumerable<DepartmentKeyValuePairDto>>(deptFromRepo);

            return Ok(deptListDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDepartment(Guid id)
        {
            var deptFromRepo = await _deptRepo.GetDepartment(id);
            var deptDto = _mapper.Map<KeyValuePairResource>(deptFromRepo);

            return Ok(deptDto);
        }

        [HttpGet]
        //[Authorize(Policy = "RequireAdminRole")]
        [Route("getDepartmentList")]
        public async Task<QueryResultResource<DepartmentDto>> GetDepartmentList(DepartmentQueryResource filterResource)
        {
            var filter = _mapper.Map<DepartmentQueryResource, DepartmentQuery>(filterResource);
            var queryResult = await _deptRepo.GetDepartmentList(filter);

            return _mapper.Map<QueryResult<DepartmentViewModel>, QueryResultResource<DepartmentDto>>(queryResult);
        }

        [Route("createDepartment")]
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto deptResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
                if (deptResource.OrganizationId == null && user.OrganizationId != null)
                    deptResource.OrganizationId = user.OrganizationId;

                if (_deptRepo.CheckDepartmentCode(deptResource.Code, deptResource.OrganizationId))
                {
                    ModelState.AddModelError("Error", "The Department with this Code already exists.");
                    return BadRequest(ModelState);
                }
                var org = await _orgRepo.GetOrganization(user.OrganizationId);
                if (org == null)
                    return NotFound();

                var dept = _mapper.Map<DepartmentDto, OrganizationDepartment>(deptResource);
                dept.Deleted = false;
                dept.Protected = false; 
                org.OrganizationDepartments.Add(dept);
                await _unitOfWork.CompleteAsync();

                dept = await _deptRepo.GetDepartment(dept.Id);
                var result = _mapper.Map<OrganizationDepartment, DepartmentDto>(dept);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditDepartment(Guid id, [FromBody] DepartmentDto deptResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var dept = await _deptRepo.GetDepartment(id);
                if (dept == null)
                {
                    return NotFound();
                }

                if (deptResource.OrganizationId == null)
                    deptResource.OrganizationId = dept.OrganizationId;
                deptResource.Code = dept.Code;
                _mapper.Map<DepartmentDto, OrganizationDepartment>(deptResource, dept);
                await _unitOfWork.CompleteAsync();

                dept = await _deptRepo.GetDepartment(id);
                var result = _mapper.Map<OrganizationDepartment, DepartmentDto>(dept);

                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var deptData = await _deptRepo.GetDepartment(id);

                if (deptData == null)
                {
                    return NotFound();
                }
                if (deptData.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be deleted");
                    return BadRequest(ModelState);
                }

                deptData.Deleted = true;
                await _unitOfWork.CompleteAsync();

                return Ok(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation. There might be some records depending on this record");
                return BadRequest(ModelState);
            }
        }
    }
}
