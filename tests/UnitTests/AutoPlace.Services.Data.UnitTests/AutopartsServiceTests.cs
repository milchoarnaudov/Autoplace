namespace AutoPlace.Services.Data.UnitTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
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

            AutoMapperConfig.RegisterMappingsThreadSafe(typeof(AutopartMockViewModel).Assembly, typeof(Autopart).Assembly);
        }

        [Fact]
        public async Task CreateAsyncSaveAutopartInDbWhenInputIsCorrect()
        {
            var list = new List<Autopart>();

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            this.autopartRepository
                .Setup(x => x.AddAsync(It.IsAny<Autopart>()))
                .Callback((Autopart autopart) => list.Add(autopart));

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            var createAutopartDto = new CreateAutopart
            {
                Name = "Test",
                Price = 23,
                Description = "Test",
                MakeYear = 2020,
                CarManufacturerId = 1,
                CarTypeId = 1,
                CategoryId = 1,
                ConditionId = 1,
                ModelId = 1,
            };

            await service.CreateAsync(createAutopartDto, "1", "/");

            Assert.Single(list);
        }

        [Fact]
        public async Task CreateAsyncDoesNotSaveAutopartInDbWhenInputIsNotCorrect()
        {
            var list = new List<Autopart>();

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            this.autopartRepository
                .Setup(x => x.AddAsync(It.IsAny<Autopart>()))
                .Callback((Autopart autopart) => list.Add(autopart));

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            var createAutopartDto = new CreateAutopart
            {
                Name = "Test",
                Price = 23,
                Description = "Test",
                MakeYear = 2020,
            };

            await service.CreateAsync(createAutopartDto, "UserId", "/path");

            Assert.Empty(list);
        }

        [Fact]
        public async Task CreateAsyncDoesNotSaveAutopartInDbWhenUserIsNull()
        {
            var list = new List<Autopart>();

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            this.autopartRepository
                .Setup(x => x.AddAsync(It.IsAny<Autopart>()))
                .Callback((Autopart autopart) => list.Add(autopart));

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            var createAutopartDto = new CreateAutopart
            {
                Name = "Test",
                Price = 23,
                Description = "Test",
                MakeYear = 2020,
            };

            await service.CreateAsync(createAutopartDto, null, "/path");

            Assert.Empty(list);
        }

        [Fact]
        public async Task AutpartsListShouldIncreaseWhenAddingNewAutopart()
        {
            var list = new List<Autopart>();

            this.autopartRepository
                .Setup(x => x.All())
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
                        CarManufacturerId = 1,
                        CarTypeId = 1,
                        ModelId = 1,
                        CategoryId = 1,
                        ConditionId = 1,
                    },
                    $"fakeUser{i}",
                    $"fakeAutopart{i}");
            }

            Assert.Equal(countOfFakeAutoparts, list.Count);
        }

        [Fact]
        public async Task EditAsyncEditsSuccessfullyWhenInputIsCorrect()
        {
            var oldValue = "old";
            var oldPrice = 1;
            var autopart = new Autopart
            {
                Id = 1,
                Description = oldValue,
                Name = oldValue,
                Price = oldPrice,
            };

            var list = new List<Autopart>
            {
                autopart,
            };

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            var newValue = "new";
            var newPrice = 2;
            var editAutopartDto = new EditAutopart
            {
                Id = autopart.Id,
                Name = newValue,
                Description = newValue,
                Price = newPrice,
            };

            var result = await service.EditAsync(editAutopartDto);

            Assert.Equal(autopart.Name, newValue);
            Assert.Equal(autopart.Description, newValue);
            Assert.Equal(autopart.Price, newPrice);
            Assert.True(result);
        }

        [Fact]
        public async Task EditAsyncDoesNotEditWhenAutopartNameIsNull()
        {
            var oldValue = "old";
            var oldPrice = 1;
            var autopart = new Autopart
            {
                Id = 1,
                Description = oldValue,
                Name = oldValue,
                Price = oldPrice,
            };

            var list = new List<Autopart>
            {
                autopart,
            };

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            var newValue = "new";
            var newPrice = 2;
            var editAutopartDto = new EditAutopart
            {
                Id = autopart.Id,
                Name = null,
                Description = newValue,
                Price = newPrice,
            };

            var result = await service.EditAsync(editAutopartDto);

            Assert.False(result);
        }

        [Fact]
        public async Task EditAsyncDoesNotEditWhenAutopartIsNull()
        {
            var oldValue = "old";
            var oldPrice = 1;
            var autopart = new Autopart
            {
                Id = 1,
                Description = oldValue,
                Name = oldValue,
                Price = oldPrice,
            };

            var list = new List<Autopart>
            {
                autopart,
            };

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            var result = await service.EditAsync(null);

            Assert.False(result);
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
                .Setup(x => x.All())
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

        [Fact]
        public async Task CheckIfUserIsOwnerReturnsTrueWhenUserIsOwner()
        {
            var list = new List<Autopart>
            {
                new Autopart
                {
                    Id = 1,
                    OwnerId = "1",
                },
            };

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            Assert.True(service.CheckIfUserIsOwner("1", 1));
        }

        [Fact]
        public async Task CheckIfUserIsOwnerReturnsFalseWhenUserIsNotOwner()
        {
            var list = new List<Autopart>
            {
                new Autopart
                {
                    Id = 1,
                    OwnerId = "1",
                },
            };

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            Assert.False(service.CheckIfUserIsOwner("2", 1));
        }

        [Fact]
        public async Task CheckIfUserIsOwnerReturnsFalseWhenUserIsNull()
        {
            var list = new List<Autopart>
            {
                new Autopart
                {
                    Id = 1,
                    OwnerId = "1",
                },
            };

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            Assert.False(service.CheckIfUserIsOwner(null, 1));
        }

        [Fact]
        public async Task CheckIfUserIsOwnerReturnsFalseWhenAutopartDoesNotExist()
        {
            var list = new List<Autopart>
            {
                new Autopart
                {
                    Id = 1,
                    OwnerId = "1",
                },
            };

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            Assert.False(service.CheckIfUserIsOwner("1", 2));
            Assert.False(service.CheckIfUserIsOwner("1", default));
        }

        [Fact]
        public async Task IncreaseViewCountIsIncreasingViewsOnAutopartWhenInputIsCorrect()
        {
            var autopart = new Autopart
            {
                Id = 1,
                CountViews = 0,
            };

            var list = new List<Autopart>
            {
                autopart,
            };

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            await service.IncreaseViewsCountAsync(1);
            await service.IncreaseViewsCountAsync(1);

            Assert.Equal(2, autopart.CountViews);
        }

        [Fact]
        public async Task IncreaseViewCountIsNotIncreasingViewsOnAutopartWhenInputIsNotCorrect()
        {
            var autopart = new Autopart
            {
                Id = 1,
                CountViews = 0,
            };

            var list = new List<Autopart>
            {
                autopart,
            };

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            await service.IncreaseViewsCountAsync(2);
            await service.IncreaseViewsCountAsync(2);

            Assert.Equal(0, autopart.CountViews);
        }

        [Fact]
        public async Task GetCountReturnsCorrectValue()
        {
            var list = new List<Autopart>
            {
               new Autopart
                {
                    Id = 1,
                    CountViews = 0,
                },
               new Autopart
                {
                    Id = 2,
                    CountViews = 0,
                },
            };

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            Assert.Equal(list.Count, service.GetCount());
        }

        [Fact]
        public async Task GetByIdReturnsCorrectValue()
        {
            var autopart = new Autopart
            {
                Id = 1,
                Name = "test",
            };

            var list = new List<Autopart>
            {
               autopart,
            };

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            var result = service.GetById<AutopartMockViewModel>(autopart.Id);

            Assert.Equal(autopart.Id, result?.Id);
            Assert.Equal(autopart.Name, result?.Name);
        }

        [Fact]
        public async Task GetByAllWithFiltersReturnAutopartsThatMatchTheFilters()
        {
            var autopartToBeMatched = new Autopart
            {
                Id = 1,
                Name = "test",
                CategoryId = 1,
                ConditionId = 1,
                Car = new Car
                {
                    Id = 1,
                    CarTypeId = 1,
                    MakeYear = 2020,
                    ModelId = 1,
                },
                Price = 1999,
            };

            var autopartToNotBeMatched = new Autopart
            {
                Id = 2,
                Name = "test",
            };

            var list = new List<Autopart>
            {
               autopartToBeMatched,
               autopartToNotBeMatched,
            };

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            var filters = new SearchFilters
            {
                CarTypeId = 1,
                CategoryId = 1,
                CarMakeYear = 2020,
                CarManufacturerId = 1,
                ConditionId = 1,
                MaxPrice = 2000,
                ModelId = 1,
            };

            var result = service.GetAll<AutopartMockViewModel>(filters);

            Assert.Single(result);
            Assert.Equal(autopartToBeMatched.Id, result.FirstOrDefault()?.Id);
            Assert.Equal(autopartToBeMatched.Name, result.FirstOrDefault()?.Name);
        }

        [Fact]
        public async Task GetByAllWithPaginationReturnCorrectCountOfAutoparts()
        {
            var list = new List<Autopart>();

            for (int i = 0; i < 20; i++)
            {
                list.Add(new Autopart
                {
                    Id = i,
                });
            }

            this.autopartRepository
                .Setup(x => x.All())
                .Returns(list.AsQueryable());

            var service = new AutopartsService(
                this.carRepository.Object,
                this.autopartRepository.Object,
                this.imageService.Object);

            var expectedResult = 5;

            var result = service.GetAll<AutopartMockViewModel>(1, expectedResult);

            Assert.Equal(expectedResult, result.Count());
        }
    }
    public class AutopartMockViewModel : IMapFrom<Autopart>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
