using IRS.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IRS.DAL.Models.Identity
{
    public partial class UserRoles: IdentityUserRole<Guid>
    {
        public User User { get; set; }
        public Roles Role { get; set; }
    }
}
