namespace AutoPlace.Web.ViewModels.Votes
{
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;

    public class VotesViewModel : IMapFrom<Vote>
    {
        public bool VoteValue { get; set; }
    }
}
