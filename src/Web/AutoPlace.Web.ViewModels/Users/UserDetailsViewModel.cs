namespace AutoPlace.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;
    using AutoPlace.Web.ViewModels.Autoparts;
    using AutoPlace.Web.ViewModels.Comments;

    public class UserDetailsViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Username { get; set; }

        public DateTime CreatedOn { get; set; }

        public int PositiveVotes { get; set; }

        public int NegativeVotes { get; set; }

        public bool IsCurrentUserVotedNegative { get; set; }

        public bool IsCurrentUserVotedPositive { get; set; }

        public IEnumerable<AutopartsListItemViewModel> Autoparts { get; set; }

        public IEnumerable<CommentListItemViewModel> CommentsForUser { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserDetailsViewModel>()
               .ForMember(x => x.PositiveVotes, opt =>
               opt.MapFrom(x => x.VotesForUser.Where(x => x.VoteValue).Count()))
               .ForMember(x => x.NegativeVotes, opt =>
               opt.MapFrom(x => x.VotesForUser.Where(x => !x.VoteValue).Count()));
        }
    }
}
