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
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };
        private readonly IDeletableEntityRepository<AutopartCategory> categoriesRepository;
        private readonly IDeletableEntityRepository<AutopartCondition> conditionsRepository;
        private readonly IDeletableEntityRepository<Car> carRepository;
        private readonly IDeletableEntityRepository<Autopart> autopartRepository;

        public AutopartsService(
            IDeletableEntityRepository<AutopartCategory> categoriesRepository,
            IDeletableEntityRepository<AutopartCondition> conditionsRepository,
            IDeletableEntityRepository<Car> carRepository,
            IDeletableEntityRepository<Autopart> autopartRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.conditionsRepository = conditionsRepository;
            this.carRepository = carRepository;
            this.autopartRepository = autopartRepository;
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

            Directory.CreateDirectory($"{imagePath}/Autoparts/");

            foreach (var image in autopart.Images)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');

                if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new Exception($"Invalid image extension {extension}");
                }

                var dbImage = new Image
                {
                    OwnerId = userId,
                    Extension = extension,
                };

                autopartEntity.Images.Add(dbImage);

                var physicalPath = $"{imagePath}/Autoparts/{dbImage.Id}.{extension}";
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }

            await this.autopartRepository.AddAsync(autopartEntity);
            await this.autopartRepository.SaveChangesAsync();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllCategoriesAsKeyValuePairs()
        {
            return this.categoriesRepository.AllAsNoTracking()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name))
                .ToList();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllConditionsAsKeyValuePairs()
        {
            return this.conditionsRepository.AllAsNoTracking()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name))
                .ToList();
        }

        public IEnumerable<T> GetAll<T>()
        {
            var recipes = this.autopartRepository.AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .To<T>().ToList();

            return recipes;
        }

        public T GetById<T>(int id)
        {
            return this.autopartRepository.AllAsNoTracking().Where(x => x.Id == id).To<T>().FirstOrDefault();
        }

        public async Task<bool> DeleteById(int id)
        {
            var autopart = this.autopartRepository.AllAsNoTracking().Where(x => x.Id == id).FirstOrDefault();

            if (autopart == null)
            {
                return false;
            }

            this.autopartRepository.Delete(autopart);
            await this.autopartRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Edit(EditAutopartDTO autopart)
        {
            var autopartEntity = this.autopartRepository.All().Where(x => x.Id == autopart.Id).FirstOrDefault();

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
    }
}
