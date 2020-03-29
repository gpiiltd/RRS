using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Dtos.UserResources
{
    public class UserDeploymentDto
    {
        public Guid? UserId { get; set; }

        public Guid? AreaOfDeploymentId { get; set; }
        public Guid? CityOfDeploymentId { get; set; }

        public Guid? StateOfDeploymentId { get; set; }

        public Guid? CountryOfDeploymentId { get; set; }

        public Guid? DepartmentId { get; set; }

        public Guid? OrganizationId { get; set; }

        public DateTime DateOfDeployment { get; set; }

        public DateTime? DateOfSignOff { get; set; }

        public bool CurrentDeployment { get; set; }

        public bool Protected { get; set; }

        public bool Deleted { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid CreatedByUserId { get; set; }

        public DateTime? DateEdited { get; set; }

        public Guid? EditedByUserId { get; set; }
    }
}
