namespace AutoPlace.Services.Data.UnitTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.Models.Autoparts;
    using AutoPlace.Services.Mapping;
    using AutoPlace.Tests.Utils;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class AutopartsServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<AutopartCategory>> categoriesRepository;
        private readonly Mock<IDeletableEntityRepository<AutopartCondition>> conditionsRepository;
        private readonly Mock<IDeletableEntityRepository<Car>> carRepository;
        private readonly Mock<IDeletableEntityRepository<Autopart>> autopartRepository;
        private readonly Mock<IImageService> imageService;

        public AutopartsServiceTests()
        {
            this.categoriesRepository = new Mock<IDeletableEntityRepository<AutopartCategory>>();
            this.conditionsRepository = new Mock<IDeletableEntityRepository<AutopartCondition>>();
            this.carRepository = new Mock<IDeletableEntityRepository<Car>>();
            this.autopartRepository = new Mock<IDeletableEntityRepository<Autopart>>();
            this.imageService = new Mock<IImageService>();
        }

        [Fact]
        public async Task AutpartsListShouldIncreaseWhenAddingNewAutopart()
        {
            var list = new List<Autopart>();

            this.autopartRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            this.autopartRepository
                .Setup(x => x.AddAsync(It.IsAny<Autopart>()))
                .Callback((Autopart autopart) => list.Add(autopart));

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);


            var file = MockObjects.GetMockFile().Object;

            var countOfFakeAutoparts = 2;

            for (int i = 0; i < countOfFakeAutoparts; i++)
            {
                await service.CreateAsync(
                    new CreateAutopart
                {
                    Name = $"Fake Autopart {i}",
                    Price = 10,
                    Images = new List<IFormFile>()
                {
                    file,
                    file,
                },
                    CarManufacturerId = 2,
                    CarTypeId = 2,
                    ModelId = 2,
                },
                    $"fakeUser{i}",
                    $"fakeAutopart{i}");
            }

            Assert.Equal(countOfFakeAutoparts, list.Count());
        }

        [Fact]
        public async Task DeleteAutopartShouldReduceTheListCount()
        {
            var list = new List<Autopart>()
            {
                new Autopart
                {
                     Id = 1,
                     Name = "A",
                },
                new Autopart
                {
                     Id = 2,
                     Name = "B",
                },
            };

            this.autopartRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable());

            this.autopartRepository
                .Setup(x => x.Delete(It.IsAny<Autopart>()))
                .Callback((Autopart autopart) => list.Remove(autopart));

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            await service.DeleteByIdAsync(1);

            Assert.Single(list);
        }
    }
}
