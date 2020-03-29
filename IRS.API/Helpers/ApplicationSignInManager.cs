using IRS.DAL.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Helpers
{
    public class ApplicationSignInManager : SignInManager<User>
    {
        //Not in use. Otherwise override SignInManager<User> to use in AuthController and Startup.cs
        public ApplicationSignInManager(UserManager<User> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<User>> logger,
            IAuthenticationSchemeProvider schemes) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {

        }

        public override async Task<bool> CanSignInAsync(User user)
        {
            //checks if user is Active or not for every login attempt
            if (UserManager.FindByIdAsync(user.UserName).Result.IsActive == false)
            {
                return false;
            }

            return false;
        }
    }
}
