namespace AutoPlace.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.DTO.Autoparts;
    using AutoPlace.Services.Mapping;

    public class AutopartsService : IAutopartsService
    {
        private readonly IDeletableEntityRepository<AutopartCategory> categoriesRepository;
        private readonly IDeletableEntityRepository<AutopartCondition> conditionsRepository;
        private readonly IDeletableEntityRepository<Car> carRepository;
        private readonly IDeletableEntityRepository<Autopart> autopartRepository;
        private readonly IImageService imageService;

        public AutopartsService(
            IDeletableEntityRepository<AutopartCategory> categoriesRepository,
            IDeletableEntityRepository<AutopartCondition> conditionsRepository,
            IDeletableEntityRepository<Car> carRepository,
            IDeletableEntityRepository<Autopart> autopartRepository,
            IImageService imageService)
        {
            this.categoriesRepository = categoriesRepository;
            this.conditionsRepository = conditionsRepository;
            this.carRepository = carRepository;
            this.autopartRepository = autopartRepository;
            this.imageService = imageService;
        }

        public async Task CreateAsync(CreateAutopartDTO autopart, string userId, string imagePath)
        {
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

            if (carEntity == null)
            {
                carEntity = new Car
                {
                    ModelId = autopart.ModelId,
                    CarTypeId = autopart.CarTypeId,
                    MakeYear = autopart.MakeYear,
                };
            }

            autopartEntity.Car = carEntity;

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

            await this.autopartRepository.AddAsync(autopartEntity);
            await this.autopartRepository.SaveChangesAsync();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAutopartCategoriesAsKeyValuePairs() =>
            this.categoriesRepository.AllAsNoTracking()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));

        public IEnumerable<KeyValuePair<string, string>> GetAllAutopartConditionsAsKeyValuePairs() =>
            this.conditionsRepository.AllAsNoTracking()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage) =>
            this.autopartRepository.AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<T>();

        public T GetById<T>(int id) =>
            this.autopartRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var autopart = this.autopartRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (autopart == null)
            {
                return false;
            }

            this.autopartRepository.Delete(autopart);
            await this.autopartRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditAsync(EditAutopartDTO autopart)
        {
            if (autopart == null)
            {
                return false;
            }

            var autopartEntity = this.autopartRepository.All()
                .Where(x => x.Id == autopart.Id)
                .FirstOrDefault();

            if (autopartEntity == null)
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

            var autopart = this.autopartRepository.AllAsNoTracking()
                .Where(x => x.Id == autopartId)
                .FirstOrDefault();

            if (autopart.OwnerId == userId)
            {
                return true;
            }

            return false;
        }

        public async Task IncreaseViewsCountAsync(int id)
        {
            var autopart = this.autopartRepository.All().Where(x => x.Id == id).FirstOrDefault();

            autopart.CountViews++;

            this.autopartRepository.Update(autopart);
            await this.autopartRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(SearchFiltersDTO searchFiltersDTO)
        {
            if (searchFiltersDTO == null)
            {
                return new List<T>();
            }

            return this.autopartRepository.AllAsNoTracking()
                .Where(x => x.ConditionId == searchFiltersDTO.ConditionId
                && x.CategoryId == searchFiltersDTO.CategoryId
                && x.Car.ModelId == searchFiltersDTO.ModelId
                && x.Car.CarTypeId == searchFiltersDTO.CarTypeId
                && (searchFiltersDTO.CarMakeYear == null || x.Car.MakeYear == searchFiltersDTO.CarMakeYear)
                && (searchFiltersDTO.MaxPrice == null || x.Price <= searchFiltersDTO.MaxPrice))
                .To<T>()
                .ToList();
        }

        public int GetCount() => this.autopartRepository.All().Count();
    }
}
