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
using IRS.API.Helpers;
using IRS.API.Helpers.Abstract;
using IRS.API.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRS.API.Controllers
{
    [Route("api/[controller]")]
    //[Produces("application/json")]
    //[ApiController]
    //[Authorize]
    public class SystemSettingsController : ControllerBase
    {
        private IGeneralSettingsRepository _settingsRepo;
        public IMapper _mapper;
        private readonly IUnitofWork _unitOfWork;

        public SystemSettingsController(IMapper mapper, IUnitofWork unitOfWork, IGeneralSettingsRepository settingsRepo)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _settingsRepo = settingsRepo;

        }

        [HttpGet]
        public async Task<IActionResult> GetSystemSettings()
        {
            var settingsFromRepo = await _settingsRepo.GetSystemSettings();
            var settings = _mapper.Map<SystemSettingsDto>(settingsFromRepo);

            return Ok(settings);
        }

        
        //[Route("api/location/createArea")]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateSystemSettings([FromBody] SystemSettingsDto systemSettingsResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var checkSettings = await _settingsRepo.CheckSystemSettings();
                if (checkSettings)
                    return BadRequest("System Settings already exists");

                var settings = _mapper.Map<SystemSettingsDto, SystemSetting>(systemSettingsResource);
                _settingsRepo.Add(settings);
                await _unitOfWork.CompleteAsync();

                settings = await _settingsRepo.GetSystemSettingsModel();
                var result = _mapper.Map<SystemSetting, SystemSettingsDto>(settings);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditSystemSettings([FromBody] SystemSettingsDto systemSettingsResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var checkSettings = await _settingsRepo.CheckSystemSettings();
                var settings = await _settingsRepo.GetSystemSettingsModel();

                //edit if exists
                if (checkSettings)
                {
                    //_settingsRepo.Remove(settings); delete
                    _mapper.Map<SystemSettingsDto, SystemSetting>(systemSettingsResource, settings);
                    await _unitOfWork.CompleteAsync();
                }

                settings = await _settingsRepo.GetSystemSettingsModel();
                var result = _mapper.Map<SystemSetting, SystemSettingsDto>(settings);

                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }
    }
}
