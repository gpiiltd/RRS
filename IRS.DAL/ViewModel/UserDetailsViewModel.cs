using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IRS.DAL.ViewModel
{
    public class UserDetailsViewModel: BaseModel
    {
        public UserDetailsViewModel()
        {
            DateOfDeployment = null;
            DateOfSignOff = null;
        }

        #region Bio Details
        public int SerialNumber { get; set; }
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

        public Guid? AreaOfOriginId { get; set; }
        public virtual Area AreaOfOrigin { get; set; }

        public virtual City CityOfOrigin { get; set; }
        public Guid? CityOfOriginId { get; set; }

        public virtual State StateOfOrigin { get; set; }
        public Guid? StateOfOriginId { get; set; }

        public virtual Country CountryOfOrigin { get; set; }
        public Guid? CountryOfOriginId { get; set; }

        public Gender Gender { get; set; }
        public string ProfilePhotoName { get; set; }
        #endregion

        #region Job Details

        public string JobTitle { get; set; }
        public string StaffNo { get; set; }
        public Guid? DepartmentId { get; set; }
        public virtual OrganizationDepartment UserDepartment { get; set; }

        public Guid? OrganizationId { get; set; }
        public virtual Organization UserOrganization { get; set; }

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

        #region User Credentials

        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsActive { get; set; }

        public string MobileAppLoginPattern { get; set; }
        #endregion

        #region Incidences
        public ICollection<Incidence> AssignedIncidences { get; set; }
        public ICollection<Incidence> AllocatedIncidences { get; set; }
        #endregion
    }
}
