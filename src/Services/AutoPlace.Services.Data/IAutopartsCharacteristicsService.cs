namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;

    using AutoPlace.Services.Data.Models.AutopartCharacteristics;

    public interface IAutopartsCharacteristicsService
    {
        IEnumerable<KeyValuePair<int, string>> GetAllCategoriesAsKeyValuePairs();

        IEnumerable<KeyValuePair<int, string>> GetAllConditionsAsKeyValuePairs();
    }
}
