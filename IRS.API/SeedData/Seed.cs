using IRS.API.Dtos.UserResources;
using IRS.DAL;
using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace IRS.API.SeedData
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Roles> _roleManager;
        public Seed(UserManager<User> userManager, RoleManager<Roles> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("SeedData/SeedUsersData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Roles>
                {
                    new Roles{Name = "Member"},
                    new Roles{Name = "Admin"},
                    new Roles{Name = "Organization Admin"},
                    new Roles{Name = "Manager"},
                    new Roles{Name = "Executive"},
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                //uncomment only when we are seeding users to the db
                //foreach (var user in users)
                //{
                //    _userManager.CreateAsync(user, "password").Wait();
                //    _userManager.AddToRoleAsync(user, "Member").Wait();
                //}

                var adminUser = new User
                {
                    Id = Guid.Parse("d273a645-afa5-4805-8455-08d6e0341d4b"),
                    UserName = "Admin",
                    FirstName = "GPI",
                    MiddleName = "",
                    LastName ="",
                    IsActive =true
                };

                IdentityResult result = _userManager.CreateAsync(adminUser, "p@ssw0rd123").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("Admin").Result;
                    _userManager.AddToRolesAsync(admin, new[] { "Admin", "Member" }).Wait();
                }
            }
        }
    }
}
