namespace AutoPlace.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.DTO.Votes;
    using Moq;
    using Xunit;

    public class VotesServiceTests
    {
        [Fact]
        public async Task WhenUserVoteOnceTheVoteIsAdded()
        {
            var list = new List<Vote>();


            var mockRepository = new Mock<IDeletableEntityRepository<Vote>>();
            mockRepository.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepository.Setup(x => x.AddAsync(It.IsAny<Vote>())).Callback(
                (Vote vote) => list.Add(vote));

            var service = new VotesService(mockRepository.Object);

            await service.AddVote(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            Assert.Single(list.Where(x => !x.IsDeleted));
        }

        [Fact]
        public async Task WhenUserVoteForSecondTimeTheVoteIsRemovedIfItIsWithSameValue()
        {
            var list = new List<Vote>();


            var mockRepository = new Mock<IDeletableEntityRepository<Vote>>();
            mockRepository.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepository.Setup(x => x.AddAsync(It.IsAny<Vote>())).Callback(
                (Vote vote) => list.Add(vote));

            var service = new VotesService(mockRepository.Object);

            await service.AddVote(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            await service.AddVote(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            Assert.Empty(list.Where(x => x.IsDeleted));
        }

        [Fact]
        public async Task WhenUserVoteForSecondTimeTheVoteIsAddedIfItHasDifferentValue()
        {
            var list = new List<Vote>();


            var mockRepository = new Mock<IDeletableEntityRepository<Vote>>();
            mockRepository.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepository.Setup(x => x.AddAsync(It.IsAny<Vote>())).Callback(
                (Vote vote) => list.Add(vote));

            var service = new VotesService(mockRepository.Object);

            await service.AddVote(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            await service.AddVote(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = false,
                VoterId = "b",
            });

            Assert.Single(list.Where(x => !x.IsDeleted));
        }
    }
}
