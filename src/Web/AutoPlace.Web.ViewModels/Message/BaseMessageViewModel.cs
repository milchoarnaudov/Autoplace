namespace AutoPlace.Web.ViewModels.Message
{
    using System.ComponentModel.DataAnnotations;

    public class BaseMessageViewModel
    {
        [StringLength(maximumLength: 100, MinimumLength = 5)]
        [Required]
        public string Topic { get; set; }

        [StringLength(maximumLength: 500, MinimumLength = 5)]
        [Required]
        public string Content { get; set; }
    }
}
