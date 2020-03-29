using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using IRS.DAL.Models.Shared;
using IRS.DAL.Models.SharedResource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Dtos.UserResources
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class UsersRolesDetailsDto
    {
        public Guid? Id { get; set; }
        public int SerialNumber { get; set; }
        #region Bio Details
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }

        public string Email1 { get; set; }
        public string Email2 { get; set; }

        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public Gender Gender { get; set; }
        #endregion

        #region Job Details

        public string JobTitle { get; set; }
        public string StaffNo { get; set; }
        public Guid? DepartmentId { get; set; }
        public virtual KeyValuePairResource UserDepartment { get; set; }

        public Guid? OrganizationId { get; set; }
        public virtual KeyValuePairResource UserOrganization { get; set; }

        public PreferredContactMethod PreferredContactMethod { get; set; }

        public Guid? UserId { get; set; }
        public virtual User User { get; set; }

        public Guid? AreaOfDeploymentId { get; set; }
        public virtual Area AreaOfDeployment { get; set; }

        public virtual City CityOfDeployment { get; set; }
        public Guid? CityOfDeploymentId { get; set; }

        public virtual State StateOfDeployment { get; set; }
        public Guid? StateOfDeploymentId { get; set; }

        public virtual Country CountryOfDeployment { get; set; }
        public Guid? CountryOfDeploymentId { get; set; }

        public DateTime? DateOfDeployment { get; set; }

        public DateTime? DateOfSignOff { get; set; }

        #endregion

        #region roles
        public string UserRoleString { get; set; }
        public string AvailableRolesString { get; set; }
        public ICollection<string> RolesForUser { get; set; }
        public ICollection<string> AvailableRoles { get; set; }
        #endregion 
    }
}
