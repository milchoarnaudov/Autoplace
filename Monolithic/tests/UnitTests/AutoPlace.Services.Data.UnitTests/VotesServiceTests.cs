﻿namespace AutoPlace.Services.Data.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.Models.Votes;
    using AutoPlace.Services.Mapping;
    using AutoPlace.Web.ViewModels.Votes;
    using Moq;
    using Xunit;

    public class VotesServiceTests
    {
        public VotesServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(VotesServiceTests).Assembly);
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

            await service.CreateAsync(new CreateVote
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

            await service.CreateAsync(new CreateVote
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            await service.CreateAsync(new CreateVote
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

            await service.CreateAsync(new CreateVote
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            await service.CreateAsync(new CreateVote
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

            await service.CreateAsync(new CreateVote
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            await service.CreateAsync(new CreateVote
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "c",
            });

            Assert.Equal(2, list.Where(x => !x.IsDeleted && x.ForUserId == "a").Count());
        }

        [Fact]
        public async Task GetAllByUsernameReturnsCorrectCount()
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

            await service.CreateAsync(new CreateVote
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "b",
            });

            await service.CreateAsync(new CreateVote
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "c",
            });

            Assert.Equal(2, service.GetAllByUserId<VotesMockModel>("a").Count());
        }

        [Fact]
        public void GetVoteShouldReturnTheCorrectVote()
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

            service.CreateAsync(new CreateVote
            {
                ForUserId = "a",
                VoteValue = true,
                VoterId = "c",
            }).GetAwaiter();

            var vote = service.GetVote<VotesMockModel>("a", "c");

            Assert.True(vote.VoteValue);
        }

        public class VotesMockModel : IMapFrom<Vote>
        {
            public int Id { get; set; }

            public bool VoteValue { get; set; }
        }
    }
}
