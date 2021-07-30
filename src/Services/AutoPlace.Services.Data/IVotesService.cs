namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.Models.Votes;

    public interface IVotesService
    {
        Task CreateAsync(CreateVote vote);

        IEnumerable<T> GetAllByUserId<T>(string userId);

        T GetVote<T>(string forUserId, string voterId);
    }
}
