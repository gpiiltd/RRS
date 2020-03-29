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
    public class StateController : ControllerBase
    {
        private IStateRepository _stateRepo;
        private ICountryRepository _countryRepo;
        private readonly IUnitofWork _unitOfWork;
        public IMapper _mapper;

        public StateController(IStateRepository stateRepo, IMapper mapper, ICountryRepository countryRepo, IUnitofWork unitOfWork)
        {
            _stateRepo = stateRepo;
            _countryRepo = countryRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("getStates")]
        public async Task<IActionResult> GetStates()
        {
            var statesFromRepo = await _stateRepo.GetStates();
            var stateListDto = _mapper.Map<IEnumerable<StateDto>>(statesFromRepo);

            return Ok(stateListDto);
        }

        [HttpGet]
        [Route("GetState/{id}")]
        public async Task<IActionResult> GetState(Guid id)
        {
            var stateFromRepo = await _stateRepo.GetState(id);
            var stateDto = _mapper.Map<StateDto>(stateFromRepo);

            return Ok(stateDto);
        }

        [HttpGet]
        [Authorize(Policy = "RequireOrgAndAdminRole")]
        [Route("getStateList")]
        public async Task<QueryResultResource<StateDetailsDto>> GetStateList(LocationQueryResource filterResource)
        {
            var filter = _mapper.Map<LocationQueryResource, LocationQuery>(filterResource);
            var queryResult = await _stateRepo.GetStateList(filter);

            return _mapper.Map<QueryResult<StateDetailsViewModel>, QueryResultResource<StateDetailsDto>>(queryResult);
        }

        [Route("createState")]
        [HttpPost]
        public async Task<IActionResult> CreateState([FromBody] StateDetailsDto stateResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var country = await _countryRepo.GetCountry(stateResource.CountryId);
                if (country == null)
                    return NotFound();

                if (_stateRepo.CheckStateCode(stateResource.StateCode))
                {
                    ModelState.AddModelError("Error", "The State with this Code already exists.");
                    return BadRequest(ModelState);
                }

                var state = _mapper.Map<StateDetailsDto, State>(stateResource);
                state.Deleted = false;
                state.Protected = false;
                country.States.Add(state);
                await _unitOfWork.CompleteAsync();

                state = await _stateRepo.GetState(state.Id);
                var result = _mapper.Map<State, StateDetailsDto>(state);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("editState/{id}")]
        public async Task<IActionResult> EditState([FromRoute] Guid id, [FromBody] StateDetailsDto stateResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var state = await _stateRepo.GetState(id);
                if (state == null)
                {
                    return NotFound();
                }

                if (state.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be edited");
                    return BadRequest(ModelState);
                }

                stateResource.StateCode = state.Code;
                _mapper.Map<StateDetailsDto, State>(stateResource, state);
                await _unitOfWork.CompleteAsync();

                state = await _stateRepo.GetState(id);
                var result = _mapper.Map<State, StateDetailsDto>(state);

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
        public async Task<IActionResult> DeleteState([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var stateData = await _stateRepo.GetState(id);

                if (stateData == null)
                {
                    return NotFound();
                }
                if (stateData.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be deleted");
                    return BadRequest(ModelState);
                }

                _stateRepo.Delete(stateData);
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
