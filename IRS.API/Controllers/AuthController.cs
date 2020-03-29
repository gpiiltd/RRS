using Microsoft.Extensions.Configuration;
using IRS.DAL.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IRS.API.Dtos.UserResources;
using IRS.DAL.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using IRS.DAL.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using IRS.API.Helpers.Abstract;
using IRS.API.Helpers;

namespace IRS.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        //ApiController substitues the need for ModelState validation and use of [FromBody] in request body
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IMediaRepository _photoRepo;

        public AuthController(IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IMediaRepository photoRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _config = config;
            _photoRepo = photoRepo;
        }
        // GET api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                //set IsActive property to true and MobileAppLoginPattern to empty string for new users
                var userToCreate = _mapper.Map<User>(userDto);

                var result = await _userManager.CreateAsync(userToCreate, userDto.Password);

                var userToReturn = _mapper.Map<UserDetailsDto>(userToCreate);

                if (result.Succeeded)
                {
                    return CreatedAtRoute("GetUser",
                        new { controller = "Users", id = userToCreate.Id }, userToReturn);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        //POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userDto.UserName);

                if (!string.IsNullOrEmpty(user.Id.ToString()) && user.Deleted != true && user.IsActive == true)
                {
                    //3rd param is to disable lockout if login fails
                    var result = await _signInManager
                        .CheckPasswordSignInAsync(user, userDto.Password, false);

                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.Users
                            .FirstOrDefaultAsync(u => u.NormalizedUserName == userDto.UserName.ToUpper());

                        var userToReturn = _mapper.Map<UserDetailsDto>(appUser);
                        var userPhotoFromRepo = await _photoRepo.GetUserProfilePhoto(user.Id);
                        userToReturn.ProfilePhotoName = userPhotoFromRepo != null ? 
                            !string.IsNullOrEmpty(userPhotoFromRepo.FileName) ? userPhotoFromRepo.FileName : ""
                            : "";
                        return Ok(new
                        {
                            token = GenerateJwtToken(appUser).Result,
                            user = userToReturn
                        });
                    }
                }
                else if (user.IsActive == false)
                {
                    ModelState.AddModelError("Error", "This account is locked");
                    return BadRequest(ModelState);
                }

                //return Unauthorized();
                ModelState.AddModelError("Error", "Unauthorized. Provide the correct username and password");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while performing this operation.");
                return BadRequest(ModelState);
            }
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Upn, user.OrganizationId.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);

            //include the role data in the token to be stored in the claim like the username and Id above
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
