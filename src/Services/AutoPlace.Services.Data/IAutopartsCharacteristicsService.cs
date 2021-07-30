namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;

    public interface IAutopartsCharacteristicsService
    {
        IEnumerable<KeyValuePair<int, string>> GetAllCategoriesAsKeyValuePairs();

        IEnumerable<KeyValuePair<int, string>> GetAllConditionsAsKeyValuePairs();
    }
}
