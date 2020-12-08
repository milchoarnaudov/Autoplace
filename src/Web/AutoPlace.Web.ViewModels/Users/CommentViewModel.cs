namespace AutoPlace.Web.ViewModels.Users
{
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;

    public class CommentViewModel : IMapFrom<Comment>
    {
        public string Content { get; set; }

        public string CommentatorUsername { get; set; }
    }
}
