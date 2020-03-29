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
using IRS.DAL.Models;
using IRS.DAL.ViewModel;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.Models.QueryResources.Users;
using IRS.DAL.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using IRS.DAL;
using IRS.DAL.Models.OrganizationAndDepartments;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IRS.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    //[Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepo;
        public IMapper _mapper;
        private readonly IUnitofWork _unitOfWork;
        private readonly IUserDeploymentRepository _userDeploymentRepo;
        private readonly IOrganizationRepository _orgRepo;
        private readonly IDepartmentRepository _deptRepo;
        private readonly UserManager<User> _userManager;
        private IHttpContextAccessor _contextAccessor;
        private ApplicationDbContext _dbContext;

        public UsersController(IUserRepository userRepo, IMapper mapper, IUnitofWork unitOfWork, IUserDeploymentRepository userDeploymentRepo, UserManager<User> userManager, IHttpContextAccessor contextAccessor, ApplicationDbContext dbContext, IOrganizationRepository orgRepo, IDepartmentRepository deptRepo)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userDeploymentRepo = userDeploymentRepo;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _dbContext = dbContext;
            _orgRepo = orgRepo;
            _deptRepo = deptRepo;
        }

        [HttpGet]
        [Route("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var usersFromRepo = await _userRepo.GetUsers();
            var userListDto = _mapper.Map<IEnumerable<UserKeyValuePairResource>>(usersFromRepo);

            return Ok(userListDto);
        }

        [HttpGet]
        [Authorize(Policy = "RequireOrgAndAdminRole")]
        [Route("getUserList")]
        public async Task<QueryResultResource<UserDetailsDto>> GetUserList(UsersQueryResource filterResource)
        {
            var filter = _mapper.Map<UsersQueryResource, UsersQuery>(filterResource);
            var queryResult = await _userRepo.GetUserList(filter);

            return _mapper.Map<QueryResult<UserDetailsViewModel>, QueryResultResource<UserDetailsDto>>(queryResult);
        }

        [HttpGet]
        //[Authorize(Policy = "RequireOrgAndAdminRole")]
        [Route("getUsersWithRoles")]
        public async Task<QueryResultResource<UsersRolesDetailsDto>> GetUsersWithRoles(UsersRolesQueryResource filterResource)
        {
            var filter = _mapper.Map<UsersRolesQueryResource, UsersRolesQuery>(filterResource);
            var queryResult = await _userRepo.GetUsersWithRoles(filter);

            return _mapper.Map<QueryResult<UsersRolesViewModel>, QueryResultResource<UsersRolesDetailsDto>>(queryResult);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var userFromRepo = await _userRepo.GetUserFullData(id);
            var userDetailsDto = _mapper.Map<UserDetailsDto>(userFromRepo);

            return Ok(userDetailsDto);
        }

        [Authorize(Policy = "RequireOrgAndAdminRole")]
        [HttpPost]
        [Route("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, [FromBody]RoleEditDto roleEditDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (string.IsNullOrEmpty(user.Id.ToString()))
                    return BadRequest();

                var userRoles = await _userManager.GetRolesAsync(user);

                var selectedRoles = roleEditDto.RoleNames;

                selectedRoles = selectedRoles ?? new string[] { };
                // disallow selectedRoles from excluding Admin role when the user is global admin - done at front end by including Admin role as readonly
                var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

                if (!result.Succeeded)
                    return BadRequest("Failed to add to roles");

                result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

                if (!result.Succeeded)
                    return BadRequest("Failed to remove the roles");

                return Ok(await _userManager.GetRolesAsync(user));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("CreateUser")]
        [Authorize(Policy = "RequireOrgAndAdminRole")]
        public async Task<IActionResult> CreateUser([FromBody] SaveUserDto saveUserDto)
        {
            try
            {
                var userToStringField = "";
                var OrgAdminUser = await _userRepo.GetUser(_userRepo.GetLoggedInUserId());
                var toStringField = saveUserDto.FirstName + " " + saveUserDto.LastName + " | " + saveUserDto.JobTitle;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (_userRepo.CheckUser(saveUserDto.Username))
                {
                    ModelState.AddModelError("Error", "A user with this username already exists.");
                    return BadRequest(ModelState);
                }

                userToStringField = toStringField;
                if (saveUserDto.OrganizationId != null)
                {
                    var orgdata = await _orgRepo.GetOrganization(saveUserDto.OrganizationId);

                    userToStringField = toStringField + " | " + orgdata.CompanyName;
                    saveUserDto.toStringField = userToStringField;
                }
                else
                {
                    var orgdata = await _orgRepo.GetOrganization(OrgAdminUser.OrganizationId);

                    userToStringField = toStringField + " | " + orgdata.CompanyName;
                    saveUserDto.toStringField = userToStringField;
                    saveUserDto.OrganizationId = OrgAdminUser.OrganizationId;
                }

                var user = _mapper.Map<SaveUserDto, User>(saveUserDto);
                user.DateCreated = DateTime.Now;
                user.CreatedByUserId = _userRepo.GetLoggedInUserId();
                user.Protected = false;
                user.Deleted = false;
                user.toStringField = userToStringField;
                user.IsActive = true;
                user.MobileAppLoginPattern = ""; //otherwise it will take null value and api will not return null values after login due to custom rules on UserDetailsDto.cs

                var result = await _userManager.CreateAsync(user, saveUserDto.Password);

                if (result.Succeeded)
                {
                    var savedUser = _userManager.FindByNameAsync(saveUserDto.Username).Result;

                    var userDeployInfo = _mapper.Map<SaveUserDto, UserDeployment>(saveUserDto);
                    userDeployInfo.UserId = savedUser.Id;
                    userDeployInfo.DateCreated = DateTime.Now;
                    userDeployInfo.Deleted = false;
                    userDeployInfo.Protected = false;
                    userDeployInfo.CreatedByUserId = _userRepo.GetLoggedInUserId();

                    _userDeploymentRepo.Add(userDeployInfo);
                }

                user = _userManager.FindByNameAsync(saveUserDto.Username).Result;
                var userData = _mapper.Map<User, UserDetailsDto>(user);


                await _unitOfWork.CompleteAsync();

                return Ok(userData);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutUser(Guid id, [FromBody] SaveUserDto saveUserDto)
        {
            try
            {
                var userToStringField = "";
                var toStringField = saveUserDto.FirstName + " " + saveUserDto.LastName + " | " + saveUserDto.JobTitle;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userRepo.GetUser(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = saveUserDto.Username.ToLower();

                userToStringField = toStringField;
                if (saveUserDto.OrganizationId != null)
                {
                    var orgdata = await _orgRepo.GetOrganization(saveUserDto.OrganizationId);
                    var orgDataDto = _mapper.Map<OrganizationDto>(orgdata);

                    userToStringField = toStringField + " | " + orgDataDto.CompanyName;
                    saveUserDto.toStringField = userToStringField;
                }
                else
                {
                    saveUserDto.toStringField = user.toStringField;
                    saveUserDto.OrganizationId = user.OrganizationId;
                }
                user.EditedByUserId = _userRepo.GetLoggedInUserId();
                user.DateEdited = DateTime.Now;
                _mapper.Map<SaveUserDto, User>(saveUserDto, user);

                //update password only if updated from the front end
                if (!string.IsNullOrEmpty(saveUserDto.Password))
                {
                    var newPassword = _userManager.PasswordHasher.HashPassword(user, saveUserDto.Password);
                    user.PasswordHash = newPassword;
                }
                await _userManager.UpdateAsync(user);

                //save deployment info; we use a different table for latter deployment history capture
                var userDeployInfo = await _userDeploymentRepo.GetUsersDeploymentInfo(id);
                if (userDeployInfo == null)
                {
                    return NotFound();
                }
                //var userDeployInfo = _mapper.Map<SaveUserDto, UserDeploymentDto>(saveUserDto);
                //To do: save the new deployment if the user's dept changes
                saveUserDto.UserId = id;
                userDeployInfo.DateEdited = DateTime.Now;
                userDeployInfo.Deleted = false;
                userDeployInfo.Protected = false;
                userDeployInfo.EditedByUserId = _userRepo.GetLoggedInUserId();
                _mapper.Map<SaveUserDto, UserDeployment>(saveUserDto, userDeployInfo);
            
                await _unitOfWork.CompleteAsync();
                user = await _userRepo.GetUser(user.Id);

                var result = _mapper.Map<User, UserDetailsDto>(user);
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
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userData = await _userRepo.GetUser(id);

                if (userData == null)
                {
                    return NotFound();
                }
                userData.Deleted = true;
                await _unitOfWork.CompleteAsync();

                return Ok(id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("getUserRoles/{userName}")]
        public async Task<IActionResult> GetUserRoles(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (string.IsNullOrEmpty(user.Id.ToString()))
                    return BadRequest();

                var userRoles = await _userManager.GetRolesAsync(user);

                return Ok(userRoles);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("updateUserPassword/{userName}")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UserDto saveUserCredentialsDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByNameAsync(saveUserCredentialsDto.UserName);
                if (user == null)
                {
                    return NotFound();
                }

                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(user, resetToken, saveUserCredentialsDto.Password);
                
                return Ok(saveUserCredentialsDto.UserName);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("updateUserLoginPattern/{userName}/{userLoginPattern}")]
        public async Task<IActionResult> UpdateUserLoginPattern([FromRoute] string userName, [FromRoute] string userLoginPattern)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return NotFound();
                }

                user.MobileAppLoginPattern = userLoginPattern;
                await _userManager.UpdateAsync(user);

                return Ok(userName);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("deactivateUser/{userName}")]
        public async Task<IActionResult> DeactivateUser([FromRoute] string userName)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return NotFound();
                }

                user.IsActive = false;
                await _userManager.UpdateAsync(user);

                return Ok(userName);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }
    }
}
