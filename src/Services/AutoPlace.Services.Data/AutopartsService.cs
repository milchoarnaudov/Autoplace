namespace AutoPlace.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.Models.Autoparts;
    using AutoPlace.Services.Mapping;

    public class AutopartsService : IAutopartsService
    {
        private readonly IDeletableEntityRepository<Car> carRepository;
        private readonly IDeletableEntityRepository<Autopart> autopartRepository;
        private readonly IImageService imageService;

        public AutopartsService(
            IDeletableEntityRepository<Car> carRepository,
            IDeletableEntityRepository<Autopart> autopartRepository,
            IImageService imageService)
        {
            this.carRepository = carRepository;
            this.autopartRepository = autopartRepository;
            this.imageService = imageService;
        }

        public async Task<int> CreateAsync(CreateAutopart autopart, string userId, string imagePath)
        {
            if (autopart is null ||
                userId is null ||
                autopart.Name is null ||
                autopart.CarManufacturerId == default ||
                autopart.ModelId == default ||
                autopart.CarTypeId == default ||
                autopart.CategoryId == default ||
                autopart.ConditionId == default)
            {
                return default;
            }

            var autopartEntity = new Autopart
            {
                Name = autopart.Name,
                Price = autopart.Price,
                Description = autopart.Description,
                CategoryId = autopart.CategoryId,
                ConditionId = autopart.ConditionId,
                OwnerId = userId,
            };

            var carEntity = this.carRepository.All()
                .FirstOrDefault(x =>
                x.ModelId == autopart.ModelId
                && x.CarTypeId == autopart.CarTypeId
                && x.MakeYear == autopart.MakeYear);

            if (carEntity is null)
            {
                carEntity = new Car
                {
                    ModelId = autopart.ModelId,
                    CarTypeId = autopart.CarTypeId,
                    MakeYear = autopart.MakeYear,
                };
            }

            autopartEntity.Car = carEntity;

            if (autopart.Images is not null)
            {
                foreach (var image in autopart.Images)
                {
                    var extension = this.imageService.GetExtension(image.FileName);

                    var dbImage = new Image
                    {
                        OwnerId = userId,
                        Extension = extension,
                    };

                    autopartEntity.Images.Add(dbImage);
                    await this.imageService.Save(image, imagePath, dbImage.Id);
                }
            }

            await this.autopartRepository.AddAsync(autopartEntity);
            await this.autopartRepository.SaveChangesAsync();

            return autopartEntity.Id;
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage) =>
            this.autopartRepository.All()
                .OrderByDescending(x => x.CreatedOn)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<T>();

        public T GetById<T>(int id) =>
            this.autopartRepository.All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var autopart = this.autopartRepository.All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (autopart is null)
            {
                return false;
            }

            this.autopartRepository.Delete(autopart);
            await this.autopartRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditAsync(EditAutopart autopart)
        {
            if (autopart is null || autopart.Name is null)
            {
                return false;
            }

            var autopartEntity = this.autopartRepository.All()
                .Where(x => x.Id == autopart.Id)
                .FirstOrDefault();

            if (autopartEntity is null)
            {
                return false;
            }

            autopartEntity.Name = autopart.Name;
            autopartEntity.Description = autopart.Description;
            autopartEntity.Price = autopart.Price;

            this.autopartRepository.Update(autopartEntity);
            await this.autopartRepository.SaveChangesAsync();

            return true;
        }

        public bool CheckIfUserIsOwner(string userId, int autopartId)
        {
            if (userId == default || autopartId == default)
            {
                return false;
            }

            var autopart = this.autopartRepository.All()
                .Where(x => x.Id == autopartId)
                .FirstOrDefault();

            if (autopart is null || autopart.OwnerId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task IncreaseViewsCountAsync(int id)
        {
            var autopart = this.autopartRepository.All().Where(x => x.Id == id).FirstOrDefault();

            if (autopart is null)
            {
                return;
            }

            autopart.CountViews++;

            this.autopartRepository.Update(autopart);
            await this.autopartRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(SearchFilters searchFilters)
        {
            if (searchFilters is null)
            {
                return new List<T>();
            }

            return this.autopartRepository.All()
                .Where(x => x.ConditionId == searchFilters.ConditionId
                && x.CategoryId == searchFilters.CategoryId
                && x.Car.ModelId == searchFilters.ModelId
                && x.Car.CarTypeId == searchFilters.CarTypeId
                && (searchFilters.CarMakeYear == null || x.Car.MakeYear == searchFilters.CarMakeYear)
                && (searchFilters.MaxPrice == null || x.Price <= searchFilters.MaxPrice))
                .To<T>()
                .ToList();
        }

        public int GetCount() => this.autopartRepository.All().Count();
    }
}
