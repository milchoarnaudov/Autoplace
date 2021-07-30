namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.Models.Votes;
    using AutoPlace.Services.Mapping;

    public class VotesService : IVotesService
    {
        private readonly IDeletableEntityRepository<Vote> votesRepository;

        public VotesService(IDeletableEntityRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public async Task<int> CreateAsync(CreateVote vote)
        {
            if (vote is null || vote.ForUserId == default || vote.VoterId == default)
            {
                return 0;
            }

            var existingVote = this.votesRepository.All()
                .Where(x => x.ForUserId == vote.ForUserId && x.VoterId == vote.VoterId)
                .FirstOrDefault();

            if (existingVote != null)
            {
                if (existingVote.VoteValue == vote.VoteValue)
                {
                    this.votesRepository.HardDelete(existingVote);
                }
                else if (existingVote.VoteValue != vote.VoteValue)
                {
                    existingVote.VoteValue = vote.VoteValue;
                }

                await this.votesRepository.SaveChangesAsync();

                return existingVote.Id;
            }

            var voteEntity = new Vote
            {
                ForUserId = vote.ForUserId,
                VoterId = vote.VoterId,
                VoteValue = vote.VoteValue,
            };

            await this.votesRepository.AddAsync(voteEntity);
            await this.votesRepository.SaveChangesAsync();

            return voteEntity.Id;
        }

        public IEnumerable<T> GetAllByUserId<T>(string id) =>
            this.votesRepository.All()
            .Where(x => x.ForUserId == id)
            .To<T>()
            .ToList();

        public T GetVote<T>(string forUserId, string voterId) =>
            this.votesRepository.All()
            .Where(x => x.ForUserId == forUserId && x.VoterId == voterId)
            .To<T>()
            .FirstOrDefault();
    }
}
