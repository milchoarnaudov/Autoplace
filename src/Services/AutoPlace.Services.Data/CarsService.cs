namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;

    public class CarsService : ICarsService
    {
        private readonly IDeletableEntityRepository<CarModel> carModelRepository;
        private readonly IDeletableEntityRepository<CarManufacturer> carManufacturerRepository;
        private readonly IDeletableEntityRepository<CarType> carTypeRepository;

        public CarsService(
            IDeletableEntityRepository<CarModel> carModelRepository,
            IDeletableEntityRepository<CarManufacturer> carManufactureRepository,
            IDeletableEntityRepository<CarType> carTypeRepository)
        {
            this.carModelRepository = carModelRepository;
            this.carManufacturerRepository = carManufactureRepository;
            this.carTypeRepository = carTypeRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllCarModelsAsKeyValuePairsById(int id)
        {
            return this.carModelRepository.AllAsNoTracking()
                .Where(x => x.ManufacturerId == id)
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllCarTypesAsKeyValuePairs()
        {
            return this.carTypeRepository.AllAsNoTracking()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllCarManufacturersAsKeyValuePairs()
        {
            return this.carManufacturerRepository.AllAsNoTracking()
             .Select(x => new
             {
                 x.Id,
                 x.Name,
             })
                .OrderBy(x => x.Name)
                .ToList().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
