namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.DTO.Autoparts;

    public interface IAutopartsService
    {
        Task CreateAsync(CreateAutopartDTO autopart, string userId, string imagePath);

        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>();

        Task<bool> Edit(EditAutopartDTO autopart);

        Task<bool> DeleteById(int id);

        IEnumerable<KeyValuePair<string, string>> GetAllCategoriesAsKeyValuePairs();

        IEnumerable<KeyValuePair<string, string>> GetAllConditionsAsKeyValuePairs();
    }
}
