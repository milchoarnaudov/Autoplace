namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.DTO.Votes;

    public interface IVotesService
    {
        Task AddVote(CreateVoteDTO vote);

        IEnumerable<T> GetAllByUsername<T>(string username);

        T GetVote<T>(string forUserId, string voterId);
    }
}
