namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.Models.AutopartCharacteristics;

    public class AutopartsCharacteristicsService : IAutopartsCharacteristicsService
    {
        private readonly IDeletableEntityRepository<AutopartCategory> categoriesRepository;
        private readonly IDeletableEntityRepository<AutopartCondition> conditionsRepository;

        public AutopartsCharacteristicsService(
            IDeletableEntityRepository<AutopartCategory> categoriesRepository,
            IDeletableEntityRepository<AutopartCondition> conditionsRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.conditionsRepository = conditionsRepository;
        }

        public IEnumerable<AutopartCharacteristic> GetAllAutopartCategories() => this.categoriesRepository.AllAsNoTracking()
                .Select(x => new AutopartCharacteristic
                {
                    Id = x.Id,
                    Value = x.Name,
                });

        public IEnumerable<AutopartCharacteristic> GetAllAutopartConditions() => this.conditionsRepository.AllAsNoTracking()
                .Select(x => new AutopartCharacteristic
                {
                    Id = x.Id,
                    Value = x.Name,
                });
    }
}
