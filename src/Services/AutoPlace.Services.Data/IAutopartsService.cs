namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;

    public interface IAutopartsService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllCategoriesAsKeyValuePairs();

        IEnumerable<KeyValuePair<string, string>> GetAllConditionsAsKeyValuePairs();
    }
}
