namespace AutoPlace.Services.Data.DTO.Votes
{
    public class CreateVoteDTO
    {
        public string ForUserId { get; set; }

        public string VoterId { get; set; }

        public bool VoteValue { get; set; }
    }
}
