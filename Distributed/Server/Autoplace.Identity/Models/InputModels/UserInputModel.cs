using Autoplace.Identity.Common;
using System.ComponentModel.DataAnnotations;

namespace Autoplace.Identity.Models.InputModels
{
    public class UserInputModel : LoginInputModel
    {
        [Required]
        [MinLength(Constants.UsernameMinLength)]
        [MaxLength(Constants.UsernameMaxLength)]
        public string Username { get; set; }
    }
}
