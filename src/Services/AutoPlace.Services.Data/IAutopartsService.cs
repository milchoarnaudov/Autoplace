namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.DTO;

    public interface IAutopartsService
    {
        Task CreateAutopartAsync(CreateAutopartDTO autopart, string userId, string imagePath);

        IEnumerable<KeyValuePair<string, string>> GetAllCategoriesAsKeyValuePairs();

        IEnumerable<KeyValuePair<string, string>> GetAllConditionsAsKeyValuePairs();
    }
}
