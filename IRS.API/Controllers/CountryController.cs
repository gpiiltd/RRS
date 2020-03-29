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
using IRS.DAL.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRS.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    //[ApiController]
    //[Authorize]
    public class CountryController : ControllerBase
    {
        private ICountryRepository _countryRepo;
        public IMapper _mapper;
        private IUnitofWork _unitOfWork;

        public CountryController(ICountryRepository countryRepo, IMapper mapper, IUnitofWork unitOfWork)
        {
            _countryRepo = countryRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("getCountries")]
        public async Task<IActionResult> GetCountries()
        {
            var countriesFromRepo = await _countryRepo.GetCountries();
            var countryListDto = _mapper.Map<IEnumerable<CountryDto>>(countriesFromRepo);

            return Ok(countryListDto);
        }

        [HttpGet]
        [Route("GetCountry/{id}")]
        public async Task<IActionResult> GetCountry(Guid id)
        {
            var countryFromRepo = await _countryRepo.GetCountry(id);
            var countryDto = _mapper.Map<CountryDto>(countryFromRepo);

            return Ok(countryDto);
        }

        [HttpGet]
        [Authorize(Policy = "RequireOrgAndAdminRole")]
        [Route("getCountryList")]
        public async Task<QueryResultResource<CountryDetailsDto>> GetCountryList(LocationQueryResource filterResource)
        {
            var filter = _mapper.Map<LocationQueryResource, LocationQuery>(filterResource);
            var queryResult = await _countryRepo.GetCountryList(filter);

            return _mapper.Map<QueryResult<CountryDetailsDto>, QueryResultResource<CountryDetailsDto>>(queryResult);
        }

        [Route("createCountry")]
        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody] CountryDetailsDto countryResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (_countryRepo.CheckCountryCode(countryResource.Code1, countryResource.Code1))
                {
                    ModelState.AddModelError("Error", "The Country with this Code already exists.");
                    return BadRequest(ModelState);
                }

                var country = _mapper.Map<CountryDetailsDto, Country>(countryResource);
                country.Deleted = false;
                country.Protected = false;
                _countryRepo.Add(country);
                await _unitOfWork.CompleteAsync();

                country = await _countryRepo.GetCountry(country.Id);
                var result = _mapper.Map<Country, CountryDetailsDto>(country);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("editCountry/{id}")]
        public async Task<IActionResult> EditCountry([FromRoute] Guid id, [FromBody] CountryDetailsDto countryResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var country = await _countryRepo.GetCountry(id);
                if (country == null)
                {
                    return NotFound();
                }

                if (country.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be edited");
                    return BadRequest(ModelState);
                }

                countryResource.Code1 = country.Code1;
                _mapper.Map<CountryDetailsDto, Country>(countryResource, country);
                await _unitOfWork.CompleteAsync();

                country = await _countryRepo.GetCountry(id);
                var result = _mapper.Map<Country, CountryDetailsDto>(country);

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
        public async Task<IActionResult> DeleteCountry([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var countryData = await _countryRepo.GetCountry(id);

                if (countryData == null)
                {
                    return NotFound();
                }
                if (countryData.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be deleted");
                    return BadRequest(ModelState);
                }

                _countryRepo.Delete(countryData);
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
