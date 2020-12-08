namespace AutoPlace.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;
    using AutoPlace.Web.ViewModels.Autoparts;

    public class UserDetailsViewModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }

        public IEnumerable<AutopartsListItemViewModel> Autoparts { get; set; }

        public IEnumerable<CommentViewModel> CommentsForUser { get; set; }
    }
}
