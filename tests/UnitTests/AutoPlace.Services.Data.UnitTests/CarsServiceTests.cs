namespace AutoPlace.Services.Data.UnitTests
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
                .Setup(x => x.All())
                .Returns(carModelsList.AsQueryable());

            var service = new CarsService(
                this.carModelsMockRepository.Object, 
                this.carManufacturersMockRepository.Object, 
                this.carTypesMockRepository.Object);

            Assert.Equal(2, service.GetAllModelsAsKeyValuePairsById(1).Count());
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
            };

            this.carModelsMockRepository
                .Setup(x => x.AllAsNoTracking())
                .Returns(carModelsList.AsQueryable());

            var service = new CarsService(
                this.carModelsMockRepository.Object, 
                this.carManufacturersMockRepository.Object, 
                this.carTypesMockRepository.Object);

            Assert.Empty(service.GetAllModelsAsKeyValuePairsById(69));
        }

        [Fact]
        public void GetAllCarManufacturersAsKeyValuePairsListCountShouldBeCorrect()
        {
            var carManufacturersList = new List<CarManufacturer>();
            var carManufacturersCount = 4;

            for (int i = 0; i < carManufacturersCount; i++)
            {
                carManufacturersList.Add(new CarManufacturer
                {
                    Id = i,
                    Name = "test",
                });
            }

            this.carManufacturersMockRepository
                .Setup(x => x.All())
                .Returns(carManufacturersList.AsQueryable());

            var service = new CarsService(
                this.carModelsMockRepository.Object,
                this.carManufacturersMockRepository.Object,
                this.carTypesMockRepository.Object);

            Assert.Equal(carManufacturersCount, service.GetAllManufacturersAsKeyValuePairs().Count());
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

            Assert.Empty(service.GetAllManufacturersAsKeyValuePairs());
        }

        [Fact]
        public void GetAllCarTypesAsKeyValuePairsListCountShouldBeCorrect()
        {
            var carTypesList = new List<CarType>();

            var carTypesCount = 6;

            for (int i = 0; i < carTypesCount; i++)
            {
                carTypesList.Add(new CarType
                {
                    Id = i,
                    Name = "test",
                });
            }

            this.carTypesMockRepository
                .Setup(x => x.All())
                .Returns(carTypesList.AsQueryable());

            var service = new CarsService(
                this.carModelsMockRepository.Object,
                this.carManufacturersMockRepository.Object,
                this.carTypesMockRepository.Object);

            Assert.Equal(6, service.GetAllTypesAsKeyValuePairs().Count());
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

            Assert.Empty(service.GetAllTypesAsKeyValuePairs());
        }
    }
}
