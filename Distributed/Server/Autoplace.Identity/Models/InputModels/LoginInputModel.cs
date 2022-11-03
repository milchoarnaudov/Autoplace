using Autoplace.Identity.Common;
using System.ComponentModel.DataAnnotations;

namespace Autoplace.Identity.Models.InputModels
{
    public class LoginInputModel
    {
        [EmailAddress]
        [Required]
        [MinLength(Constants.EmailAddressMinLength)]
        [MaxLength(Constants.EmailAddressMaxLength)]
        public string Email { get; set; }

        [Required]
        [MinLength(Constants.PasswordMinLength)]
        [MaxLength(Constants.PasswordMaxLength)]
        public string Password { get; set; }
    }
}
