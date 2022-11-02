namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;

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

        public IEnumerable<KeyValuePair<int, string>> GetAllCategoriesAsKeyValuePairs() =>
                this.categoriesRepository.All()
                    .Select(x => new KeyValuePair<int, string>(x.Id, x.Name))
                    .ToList();

        public IEnumerable<KeyValuePair<int, string>> GetAllConditionsAsKeyValuePairs() =>
                this.conditionsRepository.All()
                    .Select(x => new KeyValuePair<int, string>(x.Id, x.Name))
                    .ToList();
    }
}
