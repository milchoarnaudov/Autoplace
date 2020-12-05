namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;

    public class AutopartsService : IAutopartsService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<AutopartCondition> conditionsRepository;

        public AutopartsService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<AutopartCondition> conditionsRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.conditionsRepository = conditionsRepository;
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
    }
}
