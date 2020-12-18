namespace AutoPlace.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using Moq;
    using Xunit;

    public class FavoritesServiceTests
    {
        [Fact]
        public async Task AddToFavoritesListShouldHaveCount1WhenDoneOnce()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();
            mockRepository.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            mockRepository.Setup(x => x.AddAsync(It.IsAny<Favorite>())).Callback(
                (Favorite favorite) => list.Add(favorite));
            var service = new FavoritesService(mockRepository.Object);

            await service.AddToFavorite("a", 1);

            Assert.Single(list.Where(x => !x.IsDeleted));
        }

        [Fact]
        public async Task AddToFavoritesListShouldHaveCount0WhenDoneTwice()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();
            mockRepository.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            mockRepository.Setup(x => x.AddAsync(It.IsAny<Favorite>())).Callback(
                (Favorite favorite) => list.Add(favorite));
            mockRepository.Setup(x => x.HardDelete(It.IsAny<Favorite>())).Callback(
               (Favorite favorite) => list.Remove(favorite));
            var service = new FavoritesService(mockRepository.Object);

            await service.AddToFavorite("a", 1);
            await service.AddToFavorite("a", 1);

            Assert.Empty(list);
        }

        [Fact]
        public async Task AddToFavoritesListShouldIncreaseForCertainAutopartWhenDoneMultipleTimes()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();
            mockRepository.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            mockRepository.Setup(x => x.AddAsync(It.IsAny<Favorite>())).Callback(
                (Favorite favorite) => list.Add(favorite));
            mockRepository.Setup(x => x.HardDelete(It.IsAny<Favorite>())).Callback(
               (Favorite favorite) => list.Remove(favorite));
            var service = new FavoritesService(mockRepository.Object);

            await service.AddToFavorite("a", 1);
            await service.AddToFavorite("b", 1);
            await service.AddToFavorite("c", 1);
            await service.AddToFavorite("d", 1);

            Assert.Equal(4, list.Where(x => x.AutopartId == 1).Count());
        }

        [Fact]
        public async Task AddToFavoritesListShouldIncreaseDoneMultipleTimes()
        {
            var list = new List<Favorite>();
            var mockRepository = new Mock<IDeletableEntityRepository<Favorite>>();
            mockRepository.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            mockRepository.Setup(x => x.AddAsync(It.IsAny<Favorite>())).Callback(
                (Favorite favorite) => list.Add(favorite));
            mockRepository.Setup(x => x.HardDelete(It.IsAny<Favorite>())).Callback(
               (Favorite favorite) => list.Remove(favorite));
            var service = new FavoritesService(mockRepository.Object);

            await service.AddToFavorite("a", 1);
            await service.AddToFavorite("b", 1);
            await service.AddToFavorite("c", 1);
            await service.AddToFavorite("q", 1);
            await service.AddToFavorite("d", 2);
            await service.AddToFavorite("dx", 3);
            await service.AddToFavorite("x", 3);

            Assert.Equal(7, list.Count());
        }
    }
}
