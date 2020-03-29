using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IRS.DAL.Models.Identity
{
    public partial class Roles: IdentityRole<Guid>
    {
        public Roles()
        {
        UserRoles = new HashSet<UserRoles>();
        }

        public string Code { get; set; }

        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
