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
using Microsoft.AspNetCore.Identity;
using IRS.DAL.Models.Identity;
using IRS.API.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRS.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    //[Authorize]
    public class OrganizationController : ControllerBase
    {
        private IOrganizationRepository _orgRepo;
        public IMapper _mapper;
        private readonly IUnitofWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private IUserRepository _userRepo;

        public OrganizationController(IOrganizationRepository orgRepo, IMapper mapper, IUnitofWork unitOfWork, UserManager<User> userManager, IUserRepository userRepo)
        {
            _orgRepo = orgRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userRepo = userRepo;
        }

        [HttpGet]
        [Route("getOrganizations")]
        //[Authorize(Policy = "RequireOrgAndAdminRole")]
        public async Task<IActionResult> GetOrganizations()
        {
            var orgFromRepo = await _orgRepo.GetOrganizations();
            var orgListDto = _mapper.Map<IEnumerable<OrganizationKeyValueResource>>(orgFromRepo);

            return Ok(orgListDto);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = "RequireOrgAndAdminRole")]
        public async Task<IActionResult> GetOrganization(Guid id)
        {
            //unused for now otherwise use a dto resource that has all needed properties
            var orgFromRepo = await _orgRepo.GetOrganization(id);
            var orgDto = _mapper.Map<OrganizationDto>(orgFromRepo);

            return Ok(orgDto);
        }

        [HttpGet]
        [Authorize(Policy = "RequireOrgAndAdminRole")]
        [Route("getLoggedInUserOrganization")]
        public async Task<IActionResult> GetUserOrganization()
        {
            //used
            var user = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());

            if (user != null)
            {
                var orgFromRepo = await _orgRepo.GetOrganization(user.OrganizationId);
                var orgDto = _mapper.Map<OrganizationDto>(orgFromRepo);
                return Ok(orgDto);
            }

            ModelState.AddModelError("Error", "The user does not exist.");
            return BadRequest(ModelState);
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        [Route("getOrganizationDetailList")]
        public async Task<QueryResultResource<OrganizationDto>> GetOrganizationList(OrganizationQueryResource filterResource)
        {
            var filter = _mapper.Map<OrganizationQueryResource, OrganizationQuery>(filterResource);
            var queryResult = await _orgRepo.GetOrganizationList(filter);

            return _mapper.Map<QueryResult<OrganizationViewModel>, QueryResultResource<OrganizationDto>>(queryResult);
        }

        [Route("CreateOrganization")]
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<IActionResult> CreateOrganization([FromBody] OrganizationDto orgResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (_orgRepo.CheckOrganizationCode(orgResource.Code))
                {
                    ModelState.AddModelError("Error", "The Organization with this Code already exists.");
                    return BadRequest(ModelState);
                }

                var org = _mapper.Map<OrganizationDto, Organization>(orgResource);
                //default values
                org.Deleted = false;
                org.Protected = false;
                org.EnableBranding = false;
                org.UseSsl = true;
                org.CreatedByUserId = _userRepo.GetLoggedInUserId();
                org.DateCreated = DateTime.Now;
                //org.MaxFileSize = 10;
                //org.PageSize = 20;
                _orgRepo.Add(org);
                await _unitOfWork.CompleteAsync();

                org = await _orgRepo.GetOrganization(org.Id);
                var result = _mapper.Map<Organization, OrganizationDto>(org);
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
        [Authorize(Policy = "RequireOrgAndAdminRole")]
        public async Task<IActionResult> EditOrganization(Guid id, [FromBody] OrganizationDto orgResource)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var org = await _orgRepo.GetOrganization(id);
                if (org == null)
                {
                    return NotFound();
                }
                orgResource.Code = org.Code;
                orgResource.CreatedByUserId = org.CreatedByUserId;
                orgResource.DateCreated = org.DateCreated; 
                orgResource.Deleted = org.Deleted;
                orgResource.Protected = org.Protected;
                orgResource.EditedByUserId = _userRepo.GetLoggedInUserId();
                orgResource.DateEdited = DateTime.Now;

                _mapper.Map<OrganizationDto, Organization>(orgResource, org);
                await _unitOfWork.CompleteAsync();

                org = await _orgRepo.GetOrganization(id);
                var result = _mapper.Map<Organization, OrganizationDto>(org);

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
        public async Task<IActionResult> DeleteOrganization([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var org = await _orgRepo.GetOrganization(id);

                if (org == null)
                {
                    return NotFound();
                }
                org.Deleted = true;
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
