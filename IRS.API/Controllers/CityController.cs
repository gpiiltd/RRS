using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IRS.DAL.Infrastructure.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IRS.API.Dtos.LocationDto;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.Models.OrganizationAndDepartments;
using IRS.DAL.ViewModel;
using IRS.DAL.Models;
using Microsoft.Extensions.Logging;
using IRS.API.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRS.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    //[Authorize]
    public class CityController : ControllerBase
    {
        private ICityRepository _cityRepo;
        public IMapper _mapper;
        private readonly IUnitofWork _unitOfWork;
        private readonly IStateRepository _stateRepo;

        public CityController(ICityRepository cityRepo, IMapper mapper, IUnitofWork unitOfWork, IStateRepository stateRepo)
        {
            _cityRepo = cityRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _stateRepo = stateRepo;
        }

        [HttpGet]
        [Route("getCities")]
        public async Task<IActionResult> GetCities()
        {
            var citiesFromRepo = await _cityRepo.GetCities();
            var cityListDto = _mapper.Map<IEnumerable<CityDto>>(citiesFromRepo);

            return Ok(cityListDto);
        }

        [HttpGet]
        [Route("GetCity/{id}")]
        public async Task<IActionResult> GetCity(Guid id)
        {
            var cityFromRepo = await _cityRepo.GetCity(id);
            var cityDto = _mapper.Map<CityDto>(cityFromRepo);

            return Ok(cityDto);
        }

        [HttpGet]
        [Authorize(Policy = "RequireOrgAndAdminRole")]
        [Route("getCityList")]
        public async Task<QueryResultResource<CityDetailsDto>> GetCityList(LocationQueryResource filterResource)
        {
            var filter = _mapper.Map<LocationQueryResource, LocationQuery>(filterResource);
            var queryResult = await _cityRepo.GetCityList(filter);

            return _mapper.Map<QueryResult<CityDetailsViewModel>, QueryResultResource<CityDetailsDto>>(queryResult);
        }

        [Route("createCity")]
        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CityDetailsDto cityResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var state = await _stateRepo.GetState(cityResource.StateId);
                if (state == null)
                    return NotFound();

                if (_cityRepo.CheckCityCode(cityResource.CityCode))
                {
                    ModelState.AddModelError("Error", "The City with this Code already exists.");
                    return BadRequest(ModelState);
                }

                var city = _mapper.Map<CityDetailsDto, City>(cityResource);
                city.Deleted = false;
                city.Protected = false;
                state.Cities.Add(city);
                await _unitOfWork.CompleteAsync();

                city = await _cityRepo.GetCity(city.Id);
                var result = _mapper.Map<City, CityDetailsDto>(city);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("editCity/{id}")]
        public async Task<IActionResult> EditCity([FromRoute] Guid id, [FromBody] CityDetailsDto cityResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var city = await _cityRepo.GetCity(id);
                if (city == null)
                {
                    return NotFound();
                }

                if (city.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be edited");
                    return BadRequest(ModelState);
                }

                cityResource.CityCode = city.Code;
                _mapper.Map<CityDetailsDto, City>(cityResource, city);
                await _unitOfWork.CompleteAsync();

                city = await _cityRepo.GetCity(id);
                var result = _mapper.Map<City, CityDetailsDto>(city);

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
        public async Task<IActionResult> DeleteCity([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var cityData = await _cityRepo.GetCity(id);

                if (cityData == null)
                {
                    return NotFound();
                }
                if (cityData.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be deleted");
                    return BadRequest(ModelState);
                }

            
                _cityRepo.Delete(cityData);
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
