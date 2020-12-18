namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.DTO.Votes;
    using AutoPlace.Services.Mapping;

    public class VotesService : IVotesService
    {
        private readonly IDeletableEntityRepository<Vote> votesRepository;

        public VotesService(IDeletableEntityRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public async Task AddVote(CreateVoteDTO vote)
        {
            var existingVote = this.votesRepository.All()
                .Where(x => x.ForUserId == vote.ForUserId && x.VoterId == vote.VoterId).FirstOrDefault();

            if (existingVote == null)
            {
                var voteEntity = new Vote
                {
                    ForUserId = vote.ForUserId,
                    VoterId = vote.VoterId,
                    VoteValue = vote.VoteValue,
                };

                await this.votesRepository.AddAsync(voteEntity);
            }
            else if (existingVote.VoteValue == vote.VoteValue)
            {
                this.votesRepository.HardDelete(existingVote);
            }
            else if (existingVote.VoteValue != vote.VoteValue)
            {
                existingVote.VoteValue = vote.VoteValue;
            }

            await this.votesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllByUserId<T>(string id)
        {
            return this.votesRepository.AllAsNoTracking().Where(x => x.ForUserId == id).To<T>().ToList();
        }

        public T GetVote<T>(string forUserId, string voterId)
        {
            return this.votesRepository.AllAsNoTracking().Where(x => x.ForUserId == forUserId && x.VoterId == voterId).To<T>().FirstOrDefault();
        }
    }
}
