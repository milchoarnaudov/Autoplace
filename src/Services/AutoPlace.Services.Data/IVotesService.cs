namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.DTO.Votes;

    public interface IVotesService
    {
        Task AddVote(CreateVoteDTO vote);

        IEnumerable<T> GetAllByUserId<T>(string userId);

        T GetVote<T>(string forUserId, string voterId);
    }
}
