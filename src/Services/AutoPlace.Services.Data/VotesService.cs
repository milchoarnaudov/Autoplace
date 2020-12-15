namespace AutoPlace.Services.Data
{
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.DTO.Votes;

    public class VotesService : IVotesService
    {
        private readonly IDeletableEntityRepository<Vote> votesRepository;

        public VotesService(IDeletableEntityRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public async Task AddVote(CreateVoteDTO vote)
        {
            var voteEntity = new Vote
            {
                ForUserId = vote.ForUserId,
                VoterId = vote.VoterId,
                VoteValue = vote.VoteValue,
            };

            await this.votesRepository.AddAsync(voteEntity);
            await this.votesRepository.SaveChangesAsync();
        }
    }
}
