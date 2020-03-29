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
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.ViewModel;
using IRS.DAL.Models.QueryResources.Incidence;
using IRS.DAL.Models.QueryResource.Incidence;
using IRS.DAL.Models;
using IRS.API.Dtos.IncidenceResources;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRS.API.Controllers
{
    [Route("api/[controller]")]
    //[Produces("application/json")]
    //[ApiController]
    //[Authorize]
    public class IncidenceTypeController : ControllerBase
    {
        private IIncidenceTypeRepository _itRepo;
        public IMapper _mapper;
        private IUnitofWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private IIncidenceTypeDepartmentRepository _itdeptRepo;

        public IncidenceTypeController(IIncidenceTypeRepository itRepo, IMapper mapper, IUnitofWork unitOfWork, IUserRepository userRepo, IIncidenceTypeDepartmentRepository itdeptRepo)
        {
            _itRepo = itRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
            _itdeptRepo = itdeptRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetIncidenceTypes()
        {
            var incidenceTypeRepo = await _itRepo.GetIncidenceTypes();
            var incidenceTypeListDto = _mapper.Map<IEnumerable<IncidenceTypeDto>>(incidenceTypeRepo);

            return Ok(incidenceTypeListDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetIncidenceType(Guid id)
        {
            var incidenceTypeRepo = await _itRepo.GetIncidenceType(id);
            var incidenceTypeListDtos = _mapper.Map<IncidenceTypeDto>(incidenceTypeRepo);

            return Ok(incidenceTypeListDtos);
        }

        [HttpGet]
        //[Authorize(Policy = "RequireAdminRole")]
        [Route("getIncidenceTypesList")]
        public async Task<QueryResultResource<IncidenceTypeDto>> GetIncidenceTypesList(IncidenceTypeQueryResource filterResource)
        {
            var filter = _mapper.Map<IncidenceTypeQueryResource, IncidenceTypeQuery>(filterResource);
            var queryResult = await _itRepo.GetIncidenceTypeList(filter);

            return _mapper.Map<QueryResult<IncidenceTypeViewModel>, QueryResultResource<IncidenceTypeDto>>(queryResult);
        }

        [Route("createIncidenceType")]
        [HttpPost]
        public async Task<IActionResult> CreateIncidenceType([FromBody] IncidenceTypeDto incidenceTypeResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
                //if organizationId passed in is null, then it was passed by the organization's Admin. Hence use the organization admin's organizationId in this case
                if (incidenceTypeResource.OrganizationId == null && user.OrganizationId != null)
                    incidenceTypeResource.OrganizationId = user.OrganizationId;
                if (_itRepo.CheckIncidenceType(incidenceTypeResource.Name, incidenceTypeResource.OrganizationId))
                {
                    ModelState.AddModelError("Error", "The Incidence Type with this name already exists.");
                    return BadRequest(ModelState);
                }

                var incidenceType = _mapper.Map<IncidenceTypeDto, IncidenceType>(incidenceTypeResource);
                incidenceType.Deleted = false;
                incidenceType.Protected = false;
                _itRepo.Add(incidenceType);
                await _unitOfWork.CompleteAsync();

                incidenceType = await _itRepo.GetIncidenceType(incidenceType.Id);
                var result = _mapper.Map<IncidenceType, IncidenceTypeDto>(incidenceType);
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
        public async Task<IActionResult> EditIncidenceType([FromRoute] Guid id, [FromBody] IncidenceTypeDto incidenceTypeResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var incidenceType = await _itRepo.GetIncidenceType(id);
                if (incidenceType == null)
                {
                    return NotFound();
                }
                if (incidenceTypeResource.OrganizationId == null)
                    incidenceTypeResource.OrganizationId = incidenceType.OrganizationId;
                _mapper.Map<IncidenceTypeDto, IncidenceType>(incidenceTypeResource, incidenceType);
                await _unitOfWork.CompleteAsync();

                incidenceType = await _itRepo.GetIncidenceType(id);
                var result = _mapper.Map<IncidenceType, IncidenceTypeDto>(incidenceType);

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
        public async Task<IActionResult> DeleteIncidenceType([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var itData = await _itRepo.GetIncidenceType(id);

                if (itData == null)
                {
                    return NotFound();
                }
                if (itData.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be deleted");
                    return BadRequest(ModelState);
                }

                var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
                if (_itdeptRepo.CheckIncidenceTypeDepartment(id, user.OrganizationId))
                {
                    ModelState.AddModelError("Error", "This record is mapped to a Department. Unmap before proceeding with this operation");
                    return BadRequest(ModelState);
                }

                itData.Deleted = true;
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
