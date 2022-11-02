namespace AutoPlace.Web.ViewModels.Comments
{
    using System;

    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;

    public class CommentListItemViewModel : BaseCommentViewModel, IMapFrom<Comment>
    {
        public string CommentatorUsername { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
