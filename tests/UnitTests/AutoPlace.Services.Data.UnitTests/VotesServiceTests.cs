namespace AutoPlace.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.DTO.Votes;
    using AutoPlace.Services.Mapping;
    using AutoPlace.Web.ViewModels.Votes;
    using Moq;
    using Xunit;

    public class VotesServiceTests
    {
        public VotesServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(VotesViewModel).Assembly);
        }

        [Fact]
        public async Task WhenUserVoteOnceTheVoteIsAdded()
        {
            var list = new List<Vote>();
            var mockRepository = new Mock<IDeletableEntityRepository<Vote>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Vote>()))
                .Callback((Vote vote) => list.Add(vote));

            var service = new VotesService(mockRepository.Object);

            await service.CreateAsync(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            Assert.Single(list.Where(x => !x.IsDeleted && x.ForUserId == "a"));
        }

        [Fact]
        public async Task WhenUserVoteForSecondTimeTheVoteIsRemovedIfItIsWithSameValue()
        {
            var list = new List<Vote>();
            var mockRepository = new Mock<IDeletableEntityRepository<Vote>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Vote>()))
                .Callback((Vote vote) => list.Add(vote));

            var service = new VotesService(mockRepository.Object);

            await service.CreateAsync(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            await service.CreateAsync(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            Assert.Empty(list.Where(x => x.IsDeleted && x.ForUserId == "a"));
        }

        [Fact]
        public async Task WhenUserVoteForSecondTimeTheVoteIsAddedIfItHasDifferentValue()
        {
            var list = new List<Vote>();
            var mockRepository = new Mock<IDeletableEntityRepository<Vote>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Vote>()))
                .Callback((Vote vote) => list.Add(vote));

            var service = new VotesService(mockRepository.Object);

            await service.CreateAsync(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            await service.CreateAsync(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = false,
                VoterId = "b",
            });

            Assert.Single(list.Where(x => !x.IsDeleted && x.ForUserId == "a"));
        }

        [Fact]
        public async Task WhenTwoUsersVoteTheVotesCountIsIncreasedByTwo()
        {
            var list = new List<Vote>();
            var mockRepository = new Mock<IDeletableEntityRepository<Vote>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Vote>()))
                .Callback((Vote vote) => list.Add(vote));

            var service = new VotesService(mockRepository.Object);

            await service.CreateAsync(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            await service.CreateAsync(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "c",
            });

            Assert.Equal(2, list.Where(x => !x.IsDeleted && x.ForUserId == "a").Count());
        }

        [Fact]
        public async Task GetAllByUsernameShouldHaveCountTwo()
        {
            var list = new List<Vote>();
            var mockRepository = new Mock<IDeletableEntityRepository<Vote>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Vote>()))
                .Callback((Vote vote) => list.Add(vote));

            var service = new VotesService(mockRepository.Object);

            await service.CreateAsync(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            await service.CreateAsync(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "c",
            });

            Assert.Equal(2, service.GetAllByUserId<VotesViewModel>("a").Count());
        }

        [Fact]
        public async Task GetVoteShouldReturnTheCorrectVote()
        {
            var list = new List<Vote>();
            var mockRepository = new Mock<IDeletableEntityRepository<Vote>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Vote>()))
                .Callback((Vote vote) => list.Add(vote));

            var service = new VotesService(mockRepository.Object);

            await service.CreateAsync(new CreateVoteDTO
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "c",
            });

            var vote = service.GetVote<VotesViewModel>("a", "c");

            Assert.True(vote.VoteValue);
        }
    }
}
