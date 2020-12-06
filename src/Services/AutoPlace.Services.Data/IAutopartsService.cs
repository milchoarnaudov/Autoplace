namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.DTO;

    public interface IAutopartsService
    {
        Task CreateAutopartAsync(CreateAutopartDTO autopart, string userId, string imagePath);

        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>();

        Task<bool> DeleteById(int id);

        IEnumerable<KeyValuePair<string, string>> GetAllCategoriesAsKeyValuePairs();

        IEnumerable<KeyValuePair<string, string>> GetAllConditionsAsKeyValuePairs();
    }
}
