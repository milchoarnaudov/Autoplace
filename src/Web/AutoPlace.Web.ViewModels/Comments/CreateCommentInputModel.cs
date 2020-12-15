namespace AutoPlace.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    public class CreateCommentInputModel : BaseCommentViewModel
    {
        [Required]
        public string CommentedUserUserName { get; set; }
    }
}
