﻿namespace AutoPlace.Services.Data.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;
    using AutoPlace.Web.ViewModels.Autoparts;
    using Moq;
    using Xunit;

    public class FavoritesServiceTests
    {
        [Fact]
        public async Task AddFavoritesListShouldHaveCountOfOneWhenDoneOnce()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Favorite>()))
                .Callback(
                (Favorite favorite) => list.Add(favorite));

            var service = new FavoritesService(mockRepository.Object);

            var userId = "User_01";

            await service.AddFavoriteAsync(userId, 1);

            Assert.Single(list);
        }

        [Fact]
        public async Task AddToFavoritesTwiceRemovesIt()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Favorite>()))
                .Callback(
                (Favorite favorite) => list.Add(favorite));

            mockRepository
                .Setup(x => x.HardDelete(It.IsAny<Favorite>()))
                .Callback(
               (Favorite favorite) => list.Remove(favorite));

            var service = new FavoritesService(mockRepository.Object);

            var userId = "User_01";

            await service.AddFavoriteAsync(userId, 1);
            await service.AddFavoriteAsync(userId, 1);

            Assert.Empty(list);
        }

        [Fact]
        public async Task OneUserShouldBeAbleToAddMultipleAutopartsToFavorite()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Favorite>())).Callback(
                (Favorite favorite) => list.Add(favorite));

            mockRepository
                .Setup(x => x.HardDelete(It.IsAny<Favorite>())).Callback(
               (Favorite favorite) => list.Remove(favorite));

            var service = new FavoritesService(mockRepository.Object);

            var userId = "User_01";

            var favoritesCount = 4;

            for (int i = 1; i <= favoritesCount; i++)
            {
                await service.AddFavoriteAsync(userId, i);
            }

            Assert.Equal(favoritesCount, list.Count);
        }

        [Fact]
        public async Task OneAutopartShouldBeAbleToBeAddedToFavoritesByMultipleUsers()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Favorite>()))
                .Callback((Favorite favorite) => list.Add(favorite));

            mockRepository
                .Setup(x => x.HardDelete(It.IsAny<Favorite>()))
                .Callback((Favorite favorite) => list.Remove(favorite));

            var service = new FavoritesService(mockRepository.Object);

            var favoritesCount = 7;

            for (int i = 0; i < favoritesCount; i++)
            {
                await service.AddFavoriteAsync($"User_{i}", 3);
            }

            Assert.Equal(favoritesCount, list.Count);
        }

        [Fact]
        public async Task ReturnTrueWhenAutopartIsFavoriteForUser()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var autopartId = 123;
            var userId = "User_01";

            list.Add(new Favorite { AutopartId = autopartId, UserId = userId });

            var service = new FavoritesService(mockRepository.Object);

            var result = service.CheckIfAutopartIsFavoriteForUser(userId, autopartId);

            Assert.True(result);
        }

        [Fact]
        public async Task ReturnFalseWhenAutopartIsNotFavoriteForUser()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var autopartId = 123;
            var userId = "User_01";

            list.Add(new Favorite { AutopartId = 321, UserId = "User_02" });

            var service = new FavoritesService(mockRepository.Object);

            var result = service.CheckIfAutopartIsFavoriteForUser(userId, autopartId);

            Assert.False(result);
        }

        [Fact]
        public async Task FavoriteIsNotAddedOnNullInput()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();

            mockRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Favorite>()))
                .Callback((Favorite favorite) => list.Add(favorite));

            var service = new FavoritesService(mockRepository.Object);

            await service.AddFavoriteAsync(null, 0);
            await service.AddFavoriteAsync("Test", 0);
            await service.AddFavoriteAsync(null, 1);
            await service.AddFavoriteAsync("ToBeAdded", 1);

            Assert.Single(list);
        }
    }
}
