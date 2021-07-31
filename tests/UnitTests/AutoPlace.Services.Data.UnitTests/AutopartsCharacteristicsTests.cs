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

    public class AutopartsCharacteristicsTests
    {
        private readonly Mock<IDeletableEntityRepository<AutopartCategory>> categoriesRepository;
        private readonly Mock<IDeletableEntityRepository<AutopartCondition>> conditionsRepository;
        private readonly Mock<IDeletableEntityRepository<Car>> carRepository;
        private readonly Mock<IDeletableEntityRepository<Autopart>> autopartRepository;
        private readonly Mock<IImageService> imageService;

        public AutopartsCharacteristicsTests()
        {
            this.categoriesRepository = new Mock<IDeletableEntityRepository<AutopartCategory>>();
            this.conditionsRepository = new Mock<IDeletableEntityRepository<AutopartCondition>>();
            this.carRepository = new Mock<IDeletableEntityRepository<Car>>();
            this.autopartRepository = new Mock<IDeletableEntityRepository<Autopart>>();
            this.imageService = new Mock<IImageService>();
        }

        [Fact]
        public void GetAllCategoriesListShouldHaveCorrectCount()
        {
            var list = new List<AutopartCategory>()
            {
                new AutopartCategory
                {
                     Id = 1,
                     Name = "test",
                },
                new AutopartCategory
                {
                     Id = 2,
                     Name = "test",
                },
            };

            this.categoriesRepository.Setup(x => x.All()).Returns(list.AsQueryable());
            var service = new AutopartsCharacteristicsService(
               this.categoriesRepository.Object,
               this.conditionsRepository.Object);

            Assert.Equal(2, service.GetAllCategoriesAsKeyValuePairs().Count());
        }

        [Fact]
        public void GetAllConditionsListShouldHaveCorrectCount()
        {
            var list = new List<AutopartCondition>()
            {
                new AutopartCondition
                {
                     Id = 1,
                     Name = "test",
                },
                new AutopartCondition
                {
                     Id = 2,
                     Name = "test",
                },
            };

            this.conditionsRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsCharacteristicsService(
               this.categoriesRepository.Object,
               this.conditionsRepository.Object);

            Assert.Equal(2, service.GetAllConditionsAsKeyValuePairs().Count());
        }

        [Fact]
        public void GetAllConditionsReturnsCorrectValues()
        {
            var autopartCondition1 = new AutopartCondition
            {
                Id = 1,
                Name = "Value 1",
            };

            var autopartCondition2 = new AutopartCondition
            {
                Id = 2,
                Name = "Value 2",
            };

            var list = new List<AutopartCondition>
            {
                autopartCondition1,
                autopartCondition2,
            };

            this.conditionsRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsCharacteristicsService(
               this.categoriesRepository.Object,
               this.conditionsRepository.Object);

            var result = service.GetAllConditionsAsKeyValuePairs().ToList();

            Assert.Equal(autopartCondition1.Id, result[0].Key);
            Assert.Equal(autopartCondition2.Id, result[1].Key);

            Assert.Equal(autopartCondition1.Name, result[0].Value);
            Assert.Equal(autopartCondition2.Name, result[1].Value);
        }

        public void GetAllCategoriesReturnsCorrectValues()
        {
            var autopartCategory1 = new AutopartCategory
            {
                Id = 1,
                Name = "Value 1",
            };

            var autopartCategory2 = new AutopartCategory
            {
                Id = 2,
                Name = "Value 2",
            };

            var list = new List<AutopartCategory>
            {
                autopartCategory1,
                autopartCategory2,
            };

            this.categoriesRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsCharacteristicsService(
               this.categoriesRepository.Object,
               this.conditionsRepository.Object);

            var result = service.GetAllConditionsAsKeyValuePairs().ToList();

            Assert.Equal(autopartCategory1.Id, result[0].Key);
            Assert.Equal(autopartCategory2.Id, result[1].Key);

            Assert.Equal(autopartCategory1.Name, result[0].Value);
            Assert.Equal(autopartCategory2.Name, result[1].Value);
        }
    }
}
