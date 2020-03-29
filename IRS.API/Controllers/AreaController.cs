using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IRS.DAL.Infrastructure.Abstract;
using Microsoft.AspNetCore.Mvc;
using IRS.DAL.Models.SharedResource;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.Models;
using IRS.API.Dtos.LocationDto;
using IRS.DAL.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRS.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    //[ApiController]
    //[Authorize]
    public class AreaController : ControllerBase
    {
        private IAreaRepository _areaRepo;
        public IMapper _mapper;
        private readonly IUnitofWork _unitOfWork;
        private readonly ICityRepository _cityRepo;
        private readonly IStateRepository _stateRepo;

        public AreaController(IAreaRepository areaRepo, IMapper mapper, IUnitofWork unitOfWork, ICityRepository cityRepo, IStateRepository stateRepo)
        {
            _areaRepo = areaRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cityRepo = cityRepo;
            _stateRepo = stateRepo;
        }

        [HttpGet]
        [Route("getAreas")]
        public async Task<IActionResult> GetAreas()
        {
            var areasFromRepo = await _areaRepo.GetAreas();
            var areaListDto = _mapper.Map<IEnumerable<KeyValuePairResource>>(areasFromRepo);

            return Ok(areaListDto);
        }

        [HttpGet]
        [Route("getArea/{id}")]
        public async Task<IActionResult> GetArea(Guid id)
        {
            var areaFromRepo = await _areaRepo.GetArea(id);
            var areaDto = _mapper.Map<KeyValuePairResource>(areaFromRepo);

            return Ok(areaDto);
        }

        [HttpGet]
        [Authorize(Policy = "RequireOrgAndAdminRole")]
        [Route("getAreaList")]
        public async Task<QueryResultResource<AreaDetailsDto>> GetAreaList(LocationQueryResource filterResource)
        {
            var filter = _mapper.Map<LocationQueryResource, LocationQuery>(filterResource);
            var queryResult = await _areaRepo.GetAreaList(filter);

            return _mapper.Map<QueryResult<AreaDetailsViewModel>, QueryResultResource<AreaDetailsDto>>(queryResult);
        }

        
        [Route("createArea")]
        [HttpPost]
        public async Task<IActionResult> CreateArea([FromBody] AreaDetailsDto areaResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var state = await _stateRepo.GetState(areaResource.StateId);
                if (state == null)
                    return NotFound();
                var city = await _cityRepo.GetCity(areaResource.CityId);

                if (_areaRepo.CheckAreaCode(areaResource.AreaCode))
                {
                    ModelState.AddModelError("Error", "The Area with this Code already exists.");
                    return BadRequest(ModelState);
                }

                var area = _mapper.Map<AreaDetailsDto, Area>(areaResource);
                area.Deleted = false;
                area.Protected = false;

                state.Areas.Add(area);
                if (city != null)
                    city.Areas.Add(area);
                await _unitOfWork.CompleteAsync();

                area = await _areaRepo.GetArea(area.Id);
                var result = _mapper.Map<Area, AreaDetailsDto>(area);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing the operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("editArea/{id}")]
        public async Task<IActionResult> EditArea([FromRoute] Guid id, [FromBody] AreaDetailsDto areaResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var area = await _areaRepo.GetArea(id);
                if (area == null)
                {
                    return NotFound();
                }

                if (area.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be edited");
                    return BadRequest(ModelState);
                }

                areaResource.AreaCode = area.Code;
                _mapper.Map<AreaDetailsDto, Area>(areaResource, area);
                await _unitOfWork.CompleteAsync();

                area = await _areaRepo.GetArea(id);
                var result = _mapper.Map<Area, AreaDetailsDto>(area);

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
        public async Task<IActionResult> DeleteArea([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var areaData = await _areaRepo.GetArea(id);

                if (areaData == null)
                {
                    return NotFound();
                }
                if (areaData.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be deleted");
                    return BadRequest(ModelState);
                }

                _areaRepo.Delete(areaData);
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
