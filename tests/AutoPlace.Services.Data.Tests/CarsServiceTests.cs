namespace AutoPlace.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using Moq;
    using Xunit;

    public class CarsServiceTests
    {
        private Mock<IDeletableEntityRepository<CarModel>> carModelsMockRepository = new Mock<IDeletableEntityRepository<CarModel>>();
        private Mock<IDeletableEntityRepository<CarManufacturer>> carManufacturersMockRepository = new Mock<IDeletableEntityRepository<CarManufacturer>>();
        private Mock<IDeletableEntityRepository<CarType>> carTypesMockRepository = new Mock<IDeletableEntityRepository<CarType>>();

        public CarsServiceTests()
        {
            this.carModelsMockRepository = new Mock<IDeletableEntityRepository<CarModel>>();
            this.carManufacturersMockRepository = new Mock<IDeletableEntityRepository<CarManufacturer>>();
            this.carTypesMockRepository = new Mock<IDeletableEntityRepository<CarType>>();
        }

        [Fact]
        public void GetAllCarModelsAsKeyValuePairsByIdListCountShouldBeCorrect()
        {
            var carModelsList = new List<CarModel>
            {
                new CarModel
                {
                    Id = 1,
                    Name = "test1",
                    ManufacturerId = 1,
                },
                new CarModel
                {
                    Id = 2,
                    Name = "test1",
                    ManufacturerId = 1,
                },
                new CarModel
                {
                    Id = 2,
                    Name = "test1",
                    ManufacturerId = 2,
                },
                new CarModel
                {
                    Id = 2,
                    Name = "test1",
                    ManufacturerId = 3,
                },
            };

            this.carModelsMockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(carModelsList.AsQueryable());

            var service = new CarsService(
                this.carModelsMockRepository.Object, 
                this.carManufacturersMockRepository.Object, 
                this.carTypesMockRepository.Object);

            Assert.Equal(2, service.GetAllCarModelsAsKeyValuePairsById(1).Count());
        }

        [Fact]
        public void GetAllCarModelsAsKeyValuePairsByIdListCountShouldBeEmptyWhenThereAreNoMatches()
        {
            var carModelsList = new List<CarModel>
            {
                new CarModel
                {
                    Id = 1,
                    Name = "test1",
                    ManufacturerId = 1,
                },
                new CarModel
                {
                    Id = 2,
                    Name = "test2",
                    ManufacturerId = 1,
                },
                new CarModel
                {
                    Id = 2,
                    Name = "test3",
                    ManufacturerId = 2,
                },
                new CarModel
                {
                    Id = 2,
                    Name = "test4",
                    ManufacturerId = 3,
                },
            };

            this.carModelsMockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(carModelsList.AsQueryable());

            var service = new CarsService(
                this.carModelsMockRepository.Object, 
                this.carManufacturersMockRepository.Object, 
                this.carTypesMockRepository.Object);

            Assert.Empty(service.GetAllCarModelsAsKeyValuePairsById(69));
        }

        [Fact]
        public void GetAllCarManufacturersAsKeyValuePairsListCountShouldBeCorrect()
        {
            var carManufacturersList = new List<CarManufacturer>
            {
                new CarManufacturer
                {
                    Id = 1,
                    Name = "test1",
                },
                new CarManufacturer
                {
                    Id = 2,
                    Name = "test2",
                },
                new CarManufacturer
                {
                    Id = 3,
                    Name = "test3",
                },
                new CarManufacturer
                {
                    Id = 4,
                    Name = "test4",
                },
            };

            this.carManufacturersMockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(carManufacturersList.AsQueryable());

            var service = new CarsService(
                this.carModelsMockRepository.Object, 
                this.carManufacturersMockRepository.Object, 
                this.carTypesMockRepository.Object);

            Assert.Equal(4, service.GetAllCarManufacturersAsKeyValuePairs().Count());
        }

        [Fact]
        public void GetAllCarManufacturersAsKeyValuePairsShouldReturnEmptyList()
        {
            var carManufacturersList = new List<CarManufacturer>();

            this.carManufacturersMockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(carManufacturersList.AsQueryable());

            var service = new CarsService(
                this.carModelsMockRepository.Object,
                this.carManufacturersMockRepository.Object,
                this.carTypesMockRepository.Object);

            Assert.Empty(service.GetAllCarManufacturersAsKeyValuePairs());
        }

        [Fact]
        public void GetAllCarTypesAsKeyValuePairsListCountShouldBeCorrect()
        {
            var carTypesList = new List<CarType>
            {
                new CarType
                {
                    Id = 1,
                    Name = "test1",
                },
                new CarType
                {
                    Id = 2,
                    Name = "test2",
                },
                new CarType
                {
                    Id = 3,
                    Name = "test3",
                },
                new CarType
                {
                    Id = 4,
                    Name = "test4",
                },
                new CarType
                {
                    Id = 5,
                    Name = "test5",
                },
                new CarType
                {
                    Id = 6,
                    Name = "test6",
                },
            };

            this.carTypesMockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(carTypesList.AsQueryable());

            var service = new CarsService(
                this.carModelsMockRepository.Object,
                this.carManufacturersMockRepository.Object,
                this.carTypesMockRepository.Object);

            Assert.Equal(6, service.GetAllCarTypesAsKeyValuePairs().Count());
        }

        [Fact]
        public void GetAllCarTypesAsKeyValuePairsShouldReturnEmptyList()
        {
            var carTypesList = new List<CarType>();

            this.carTypesMockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(carTypesList.AsQueryable());

            var service = new CarsService(
                this.carModelsMockRepository.Object,
                this.carManufacturersMockRepository.Object,
                this.carTypesMockRepository.Object);

            Assert.Empty(service.GetAllCarTypesAsKeyValuePairs());
        }
    }
}
