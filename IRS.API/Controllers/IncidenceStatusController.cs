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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRS.API.Controllers
{
    [Route("api/[controller]")]
    //[Produces("application/json")]
    //[ApiController]
    //[Authorize]
    public class IncidenceStatusController : ControllerBase
    {
        private IIncidenceStatusRepository _isRepo;
        public IMapper _mapper;
        private IUnitofWork _unitOfWork;
        public IncidenceStatusController(IIncidenceStatusRepository isRepo, IMapper mapper, IUnitofWork unitOfWork)
        {
            _isRepo = isRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetIncidenceStatuses()
        {
            var incidenceStatusRepo = await _isRepo.GetIncidenceStatuses();
            var incidenceStatusListDto = _mapper.Map<IEnumerable<IncidenceStatusDto>>(incidenceStatusRepo);

            return Ok(incidenceStatusListDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetIncidenceStatus(Guid id)
        {
            var incidenceStatusRepo = await _isRepo.GetIncidenceStatus(id);
            var incidenceStatusListDtos = _mapper.Map<IncidenceStatusDto>(incidenceStatusRepo);

            return Ok(incidenceStatusListDtos);
        }

        [HttpGet]
        //[Authorize(Policy = "RequireAdminRole")]
        [Route("getIncidenceStatusList")]
        public async Task<QueryResultResource<IncidenceStatusDto>> GetIncidenceStatusList(IncidenceStatusQueryResource filterResource)
        {
            var filter = _mapper.Map<IncidenceStatusQueryResource, IncidenceStatusQuery>(filterResource);
            var queryResult = await _isRepo.GetIncidenceStatusList(filter);

            return _mapper.Map<QueryResult<IncidenceStatusViewModel>, QueryResultResource<IncidenceStatusDto>>(queryResult);
        }

        [Route("createIncidenceStatus")]
        [HttpPost]
        public async Task<IActionResult> CreateIncidenceStatus([FromBody] IncidenceStatusDto incidenceStatusResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (_isRepo.CheckIncidenceStatus(incidenceStatusResource.Name))
                {
                    ModelState.AddModelError("Error", "The Incidence Status with this name already exists.");
                    return BadRequest(ModelState);
                }

                var incidenceStatus = _mapper.Map<IncidenceStatusDto, IncidenceStatus>(incidenceStatusResource);
                incidenceStatus.Deleted = false;
                incidenceStatus.Protected = false;
                _isRepo.Add(incidenceStatus);
                await _unitOfWork.CompleteAsync();

                incidenceStatus = await _isRepo.GetIncidenceStatus(incidenceStatus.Id);
                var result = _mapper.Map<IncidenceStatus, IncidenceStatusDto>(incidenceStatus);
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
        public async Task<IActionResult> EditIncidenceStatus([FromRoute] Guid id, [FromBody] IncidenceStatusDto incidenceStatusResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var incidenceStatus = await _isRepo.GetIncidenceStatus(id);
                if (incidenceStatus == null)
                {
                    return NotFound();
                }
                _mapper.Map<IncidenceStatusDto, IncidenceStatus>(incidenceStatusResource, incidenceStatus);
                await _unitOfWork.CompleteAsync();

                incidenceStatus = await _isRepo.GetIncidenceStatus(id);
                var result = _mapper.Map<IncidenceStatus, IncidenceStatusDto>(incidenceStatus);

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
        public async Task<IActionResult> DeleteIncidenceStatus([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isData = await _isRepo.GetIncidenceStatus(id);

                if (isData == null)
                {
                    return NotFound();
                }
                if (isData.Protected == true)
                {
                    ModelState.AddModelError("Error", "This record is a protected record. It can not be deleted");
                    return BadRequest(ModelState);
                }

                _isRepo.Delete(isData);
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
