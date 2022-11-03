using Autoplace.Identity.Common;
using System.ComponentModel.DataAnnotations;

namespace Autoplace.Identity.Models.InputModels
{
    public class ForgottenPasswordInputModel
    {
        [EmailAddress]
        [Required]
        [MinLength(Constants.EmailAddressMinLength)]
        [MaxLength(Constants.EmailAddressMaxLength)]
        public string Email { get; set; }
    }
}
