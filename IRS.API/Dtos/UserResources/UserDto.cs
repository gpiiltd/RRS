using System.ComponentModel.DataAnnotations;

namespace IRS.API.Dtos.UserResources
{
    public class UserDto
    {
        [Required]
        public string UserName{get; set;}

        [Required]
        [StringLength(12, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 10 characters")]
        public string Password {get; set;}
    }
}