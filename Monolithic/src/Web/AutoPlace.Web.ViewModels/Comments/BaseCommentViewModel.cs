namespace AutoPlace.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    public class BaseCommentViewModel
    {
        [StringLength(maximumLength: 300, MinimumLength = 5)]
        [Required]
        public string Content { get; set; }
    }
}
