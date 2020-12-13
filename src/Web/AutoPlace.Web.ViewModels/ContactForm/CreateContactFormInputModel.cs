namespace AutoPlace.Web.ViewModels.ContactForm
{
    using System.ComponentModel.DataAnnotations;

    public class CreateContactFormInputModel
    {
        [Display(Name = "Full Name")]
        [StringLength(maximumLength: 255, MinimumLength = 5)]
        [Required]
        public string FullName { get; set; }

        [StringLength(maximumLength: 255, MinimumLength = 5)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 5)]
        [Required]
        public string Topic { get; set; }

        [StringLength(maximumLength: 500, MinimumLength = 10)]
        [Required]
        public string Message { get; set; }
    }
}
