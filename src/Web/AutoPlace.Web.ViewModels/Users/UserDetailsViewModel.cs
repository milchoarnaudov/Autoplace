namespace AutoPlace.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;

    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;
    using AutoPlace.Web.ViewModels.Autoparts;
    using AutoPlace.Web.ViewModels.Comments;

    public class UserDetailsViewModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<AutopartsListItemViewModel> Autoparts { get; set; }

        public IEnumerable<CommentListItemViewModel> CommentsForUser { get; set; }
    }
}
