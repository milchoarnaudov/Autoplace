namespace AutoPlace.Services.Data.Tests
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
        public async Task AddToFavoritesListShouldHaveCountOfOneWhenDoneOnce()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Favorite>()))
                .Callback(
                (Favorite favorite) => list.Add(favorite));

            var service = new FavoritesService(mockRepository.Object);

            await service.AddToFavorite("a", 1);

            Assert.Single(list);
        }

        [Fact]
        public async Task AddToFavoritesListShouldBeEmptyAutopartAddedToFavoritesTwice()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
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

            await service.AddToFavorite("a", 1);
            await service.AddToFavorite("a", 1);

            Assert.Empty(list);
        }

        [Fact]
        public async Task OneUserShouldBeAbleToAddMultipleAutoparts()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Favorite>())).Callback(
                (Favorite favorite) => list.Add(favorite));

            mockRepository
                .Setup(x => x.HardDelete(It.IsAny<Favorite>())).Callback(
               (Favorite favorite) => list.Remove(favorite));

            var service = new FavoritesService(mockRepository.Object);

            var favoritesCount = 4;

            for (int i = 0; i < favoritesCount; i++)
            {
                await service.AddToFavorite("fakeUser", i);
            }

            Assert.Equal(favoritesCount, list.Count);
        }

        [Fact]
        public async Task OneAutopartShouldBeAbleToBeAddedToFavoritesByMultipleUsers()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();

            mockRepository
                .Setup(x => x.AllAsNoTracking())
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
                await service.AddToFavorite($"{i}", 3);
            }

            Assert.Equal(favoritesCount, list.Count);
        }
    }
}
