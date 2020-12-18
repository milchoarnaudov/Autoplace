namespace AutoPlace.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.DTO.Autoparts;
    using AutoPlace.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class AutopartsServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<AutopartCategory>> categoriesRepository;
        private readonly Mock<IDeletableEntityRepository<AutopartCondition>> conditionsRepository;
        private readonly Mock<IDeletableEntityRepository<Car>> carRepository;
        private readonly Mock<IDeletableEntityRepository<Autopart>> autopartRepository;

        public AutopartsServiceTests()
        {
            this.categoriesRepository = new Mock<IDeletableEntityRepository<AutopartCategory>>();
            this.conditionsRepository = new Mock<IDeletableEntityRepository<AutopartCondition>>();
            this.carRepository = new Mock<IDeletableEntityRepository<Car>>();
            this.autopartRepository = new Mock<IDeletableEntityRepository<Autopart>>();

            AutoMapperConfig.RegisterMappings(typeof(AutopartMap).Assembly);
        }

        [Fact]
        public async Task AutpartsListShouldIncreaseWhenAddingNewAutopart()
        {
            var list = new List<Autopart>();
            this.autopartRepository.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            this.autopartRepository.Setup(x => x.AddAsync(It.IsAny<Autopart>())).Callback(
                (Autopart autopart) => list.Add(autopart));
            var service = new AutopartsService(
                this.categoriesRepository.Object,
                this.conditionsRepository.Object,
                this.carRepository.Object,
                this.autopartRepository.Object);
            var fileMock = new Mock<IFormFile>();
            var content = "test";
            var fileName = "test.png";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var file = fileMock.Object;
            var autopartA = new CreateAutopartDTO
            {
                Name = "TestA",
                Price = 5,
                Images = new List<IFormFile>()
                {
                    file,
                    file,
                },
                CarManufacturerId = 1,
                CarTypeId = 1,
                ModelId = 1,
            };

            var autopartB = new CreateAutopartDTO
            {
                Name = "TestBA",
                Price = 10,
                Images = new List<IFormFile>()
                {
                    file,
                    file,
                },
                CarManufacturerId = 2,
                CarTypeId = 2,
                ModelId = 2,
            };

            await service.CreateAutopartAsync(autopartA, "a", "testA");
            await service.CreateAutopartAsync(autopartB, "a", "testB");

            Assert.Equal(2, list.Count());
        }

        [Fact]
        public async Task GetAllAutopartsShouldReturnCorrectCount()
        {
            var list = new List<Autopart>();
            this.autopartRepository.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            this.autopartRepository.Setup(x => x.AddAsync(It.IsAny<Autopart>())).Callback(
                (Autopart autopart) => list.Add(autopart));
            var service = new AutopartsService(
                this.categoriesRepository.Object,
                this.conditionsRepository.Object,
                this.carRepository.Object,
                this.autopartRepository.Object);

            var file = this.GetMockFile().Object;
            var autopartA = new CreateAutopartDTO
            {
                Name = "TestA",
                Price = 5,
                Images = new List<IFormFile>()
                {
                    file,
                    file,
                },
                CarManufacturerId = 1,
                CarTypeId = 1,
                ModelId = 1,
            };
            var autopartB = new CreateAutopartDTO
            {
                Name = "TestBA",
                Price = 10,
                Images = new List<IFormFile>()
                {
                    file,
                    file,
                },
                CarManufacturerId = 2,
                CarTypeId = 2,
                ModelId = 2,
            };
            await service.CreateAutopartAsync(autopartA, "a", "testA");
            await service.CreateAutopartAsync(autopartB, "a", "testB");

            Assert.Equal(2, service.GetAllAutoparts<AutopartMap>().Count());
        }

        [Fact]
        public void GetAllCategoriesAsKeyValuePairsListShouldHaveCorrectCount()
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

            this.categoriesRepository.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            var service = new AutopartsService(
               this.categoriesRepository.Object,
               this.conditionsRepository.Object,
               this.carRepository.Object,
               this.autopartRepository.Object);

            Assert.Equal(2, service.GetAllAutopartCategoriesAsKeyValuePairs().Count());
        }

        [Fact]
        public void GetAllConditionsAsKeyValuePairsListShouldHaveCorrectCount()
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

            this.conditionsRepository.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            var service = new AutopartsService(
               this.categoriesRepository.Object,
               this.conditionsRepository.Object,
               this.carRepository.Object,
               this.autopartRepository.Object);

            Assert.Equal(2, service.GetAllAutopartConditionsAsKeyValuePairs().Count());
        }

        [Fact]
        public void GetAutopartByIdShouldReturnTheCorrectAutopart()
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

            this.autopartRepository.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            var service = new AutopartsService(
                this.categoriesRepository.Object,
                this.conditionsRepository.Object,
                this.carRepository.Object,
                this.autopartRepository.Object);


            Assert.Equal("A", service.GetAutopartById<AutopartMap>(1).Name);
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

            this.autopartRepository.Setup(x => x.AllAsNoTracking()).Returns(list.AsQueryable());
            this.autopartRepository.Setup(x => x.Delete(It.IsAny<Autopart>())).Callback(
               (Autopart autopart) => list.Remove(autopart));

            var service = new AutopartsService(
                this.categoriesRepository.Object,
                this.conditionsRepository.Object,
                this.carRepository.Object,
                this.autopartRepository.Object);


            await service.DeleteAutopartByIdAsync(1);

            Assert.Single(list);
        }

        private Mock<IFormFile> GetMockFile()
        {
            var fileMock = new Mock<IFormFile>();
            var content = "test";
            var fileName = "test.png";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            return fileMock;
        }

        public class AutopartMap : IMapFrom<Autopart>
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
