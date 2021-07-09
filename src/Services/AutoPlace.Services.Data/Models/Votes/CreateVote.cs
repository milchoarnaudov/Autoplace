namespace AutoPlace.Services.Data.Models.Votes
{
    public class CreateVote
    {
        public string ForUserId { get; set; }

        public string VoterId { get; set; }

        public bool VoteValue { get; set; }
    }
}
