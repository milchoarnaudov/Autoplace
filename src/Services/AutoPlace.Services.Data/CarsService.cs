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

        public IEnumerable<KeyValuePair<int, string>> GetAllModelsAsKeyValuePairsById(int id) =>
            this.carModelRepository.All()
                .Where(x => x.ManufacturerId == id)
                .Select(x => new KeyValuePair<int, string>(x.Id, x.Name))
                .ToList();

        public IEnumerable<KeyValuePair<int, string>> GetAllTypesAsKeyValuePairs() =>
            this.carTypeRepository.All()
                .Select(x => new KeyValuePair<int, string>(x.Id, x.Name))
                .ToList();

        public IEnumerable<KeyValuePair<int, string>> GetAllManufacturersAsKeyValuePairs() =>
            this.carManufacturerRepository.All()
                .OrderBy(x => x.Name)
                .Select(x => new KeyValuePair<int, string>(x.Id, x.Name))
                .ToList();
    }
}
