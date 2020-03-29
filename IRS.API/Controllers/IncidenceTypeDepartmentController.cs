using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IRS.DAL.Infrastructure.Abstract;
using Microsoft.AspNetCore.Mvc;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.ViewModel;
using IRS.DAL.Models.QueryResources.Incidence;
using IRS.DAL.Models.QueryResource.Incidence;
using IRS.DAL.Models;
using IRS.API.Dtos.IncidenceResources;
using IRS.DAL.Infrastructure;
using IRS.DAL.Models.SharedResource;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRS.API.Controllers
{
    [Route("api/[controller]")]
    //[Produces("application/json")]
    //[ApiController]
    //[Authorize]
    public class IncidenceTypeDepartmentController : ControllerBase
    {
        private IIncidenceTypeDepartmentRepository _itdRepo;
        public IMapper _mapper;
        private IUnitofWork _unitOfWork;
        private readonly IUserRepository _userRepo;

        public IncidenceTypeDepartmentController(IIncidenceTypeDepartmentRepository itdRepo, IMapper mapper, IUnitofWork unitOfWork, IUserRepository userRepo)
        {
            _itdRepo = itdRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
        }

        [HttpGet]
        [Route("getIncidenceTypeDepartment/{id}")]
        public async Task<IActionResult> GetIncidenceTypeDepartment(Guid id)
        {
            var incidenceTypeDeptRepo = await _itdRepo.GetIncidenceTypeDepartment(id);
            var incidenceTypeDeptDto = _mapper.Map<IncidenceTypeDepartmentDto>(incidenceTypeDeptRepo);

            return Ok(incidenceTypeDeptDto);
        }

        [HttpGet]
        //[Authorize(Policy = "RequireAdminRole")]
        [Route("getIncidenceTypesList")]
        public async Task<QueryResultResource<IncidenceTypeDepartmentDto>> GetIncidenceTypesDepartmentsList(IncidenceTypeDepartmentQueryResource filterResource)
        {
            var filter = _mapper.Map<IncidenceTypeDepartmentQueryResource, IncidenceTypeDepartmentQuery>(filterResource);
            var queryResult = await _itdRepo.GetIncidenceTypesDepartmentsList(filter);

            return _mapper.Map<QueryResult<IncidenceTypeDepartmentViewModel>, QueryResultResource<IncidenceTypeDepartmentDto>>(queryResult);
        }

        [Route("createIncidenceTypeDepartment")]
        [HttpPost]
        public async Task<IActionResult> CreateIncidenceTypeDepartment([FromBody] IncidenceTypeDepartmentDto incidenceTypeDeptResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
                //if organizationId passed in is null, then it was passed by the organization's Admin. Hence use the organization admin's organizationId in this case
                if (incidenceTypeDeptResource.OrganizationId == null && user.OrganizationId != null)
                    incidenceTypeDeptResource.OrganizationId = user.OrganizationId;

                if (_itdRepo.CheckIncidenceTypeDepartment(incidenceTypeDeptResource.IncidenceTypeId, incidenceTypeDeptResource.OrganizationId))
                {
                    ModelState.AddModelError("Error", "This Incident Type is already mapped to a department.");
                    return BadRequest(ModelState);
                }

                var incidenceTypeDept = _mapper.Map<IncidenceTypeDepartmentDto, IncidenceTypeDepartmentMapping>(incidenceTypeDeptResource);
                _itdRepo.Add(incidenceTypeDept);
                await _unitOfWork.CompleteAsync();

                incidenceTypeDept = await _itdRepo.GetIncidenceTypeDepartment(incidenceTypeDept.Id);
                var result = _mapper.Map<IncidenceTypeDepartmentMapping, IncidenceTypeDepartmentDto>(incidenceTypeDept);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("editIncidenceTypeDepartment/{id}")]
        public async Task<IActionResult> EditIncidenceTypeDepartment([FromRoute] Guid id, [FromBody] IncidenceTypeDepartmentDto incidenceTypeDeptResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var incidenceTypeDept = await _itdRepo.GetIncidenceTypeDepartment(id);
                if (incidenceTypeDept == null)
                {
                    return NotFound();
                }

                //organizationId can only change when Global admin passed it in. If not passed in, use same saved user's organizationId
                if (incidenceTypeDeptResource.OrganizationId == null)
                    incidenceTypeDeptResource.OrganizationId = incidenceTypeDept.OrganizationId;

                _mapper.Map<IncidenceTypeDepartmentDto, IncidenceTypeDepartmentMapping>(incidenceTypeDeptResource, incidenceTypeDept);
                await _unitOfWork.CompleteAsync();

                incidenceTypeDept = await _itdRepo.GetIncidenceTypeDepartment(id);
                var result = _mapper.Map<IncidenceTypeDepartmentMapping, IncidenceTypeDepartmentDto>(incidenceTypeDept);

                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("getUnmappedIncidenceTypesForUser")]
        public async Task<IActionResult> GetUnmappedIncidenceTypes()
        {
            var unmappedIncidenceTypesFromRepo = await _itdRepo.GetUnmappedIncidenceTypes();
            var unmappedIncidenceTypesFromRepoDtos = _mapper.Map<IEnumerable<KeyValuePairResource>>(unmappedIncidenceTypesFromRepo);

            return Ok(unmappedIncidenceTypesFromRepoDtos);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteIncidenceTypeDepartment([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var itdData = await _itdRepo.GetIncidenceTypeDepartment(id);

                if (itdData == null)
                {
                    return NotFound();
                }

                _itdRepo.Delete(itdData);
                await _unitOfWork.CompleteAsync();

                return Ok(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }
    }
}
