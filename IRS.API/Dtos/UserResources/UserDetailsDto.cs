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
    public class UserDetailsDto
    {
        public Guid? Id { get; set; }
        public int SerialNumber { get; set; }
        #region Bio Details
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        public string Email1 { get; set; }
        public string Email2 { get; set; }

        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime? LastActive { get; set; }
        public string Introduction { get; set; }

        public Guid? AreaOfOriginId { get; set; }
        public virtual KeyValuePairResource AreaOfOrigin { get; set; }

        public virtual KeyValuePairResource CityOfOrigin { get; set; }
        public Guid? CityOfOriginId { get; set; }

        public virtual KeyValuePairResource StateOfOrigin { get; set; }
        public Guid? StateOfOriginId { get; set; }

        public virtual KeyValuePairResource CountryOfOrigin { get; set; }
        public Guid? CountryOfOriginId { get; set; }

        public Gender Gender { get; set; }
        public string ProfilePhotoName { get; set; }
        #endregion

        #region Job Details

        public string JobTitle { get; set; }
        public string StaffNo { get; set; }

        public Guid? DepartmentId { get; set; }
        public virtual KeyValuePairResource UserDepartment { get; set; }

        public Guid? OrganizationId { get; set; }
        public virtual KeyValuePairResource UserOrganization { get; set; }

        public PreferredContactMethod PreferredContactMethod { get; set; }

        public Guid? UserId { get; set; }//
        public virtual User User { get; set; }

        public Guid? AreaOfDeploymentId { get; set; }
        public virtual KeyValuePairResource AreaOfDeployment { get; set; }

        public virtual KeyValuePairResource CityOfDeployment { get; set; }
        public Guid? CityOfDeploymentId { get; set; }

        public virtual KeyValuePairResource StateOfDeployment { get; set; }
        public Guid? StateOfDeploymentId { get; set; }

        public virtual KeyValuePairResource CountryOfDeployment { get; set; }
        public Guid? CountryOfDeploymentId { get; set; }

        public DateTime DateOfDeployment { get; set; }

        public DateTime? DateOfSignOff { get; set; }

        #endregion

        #region User Credentials

        public string UserName { get; set; }
        //public byte[] PasswordHash { get; set; }
        //public byte[] PasswordSalt { get; set; }

        #endregion

        #region Incidences
        // not mapped currently, mapping will be implemented in a different dto i.e UserIncidenceResource
        public ICollection<UserIncidenceResource> AssignedIncidences { get; set; } // change KeyValuePairResource to UserIncidenceResource in case more info is needed than just Id and Name
        public ICollection<UserIncidenceResource> AllocatedIncidences { get; set; }

        public UserDetailsDto()
        {
            AllocatedIncidences = new HashSet<UserIncidenceResource>();
            AssignedIncidences = new HashSet<UserIncidenceResource>();
        }

        public bool IsActive { get; set; }
        public string MobileAppLoginPattern { get; set; }
        #endregion
    }
}
