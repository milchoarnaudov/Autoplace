namespace AutoPlace.Services.Data
{
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.DTO.Votes;

    public interface IVotesService
    {
        Task AddVote(CreateVoteDTO vote);
    }
}
