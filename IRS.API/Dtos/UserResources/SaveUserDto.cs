using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using IRS.DAL.Models.SharedResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Dtos.UserResources
{

    public class SaveUserDto
    {
        public Guid? Id { get; set; }
        #region Bio Details
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string Email1 { get; set; }
        public string Email2 { get; set; }

        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime? LastActive { get; set; }
        public string Introduction { get; set; }

        //public Guid? AreaOfOriginId { get; set; }

        //public Guid? CityOfOriginId { get; set; }

        //public Guid? StateOfOriginId { get; set; }

        //public Guid? CountryOfOriginId { get; set; }

        public Gender Gender { get; set; }
        public string PhotoUrl { get; set; }
        #endregion

        #region Job Details

        public string JobTitle { get; set; }
        public string StaffNo { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? OrganizationId { get; set; }

        public PreferredContactMethod PreferredContactMethod { get; set; }

        public Guid? UserId { get; set; }

        public Guid? AreaOfDeploymentId { get; set; }

        public Guid? CityOfDeploymentId { get; set; }

        public Guid? StateOfDeploymentId { get; set; }

        public Guid? CountryOfDeploymentId { get; set; }

        public DateTime DateOfDeployment { get; set; }

        public DateTime? DateOfSignOff { get; set; }

        public string toStringField { get; set; }

        public bool IsActive { get; set; }
        public string MobileAppLoginPattern { get; set; }

        #endregion

        #region User Credentials

        public string Username { get; set; }
        public string Password { get; set; }

        #endregion
    }
}
