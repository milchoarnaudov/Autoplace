namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.DTO.Autoparts;

    public interface IAutopartsService
    {
        Task CreateAutopartAsync(CreateAutopartDTO autopart, string userId, string imagePath);

        T GetAutopartById<T>(int id);

        IEnumerable<T> GetAllAutoparts<T>();

        Task<bool> EditAutopart(EditAutopartDTO autopart);

        Task<bool> DeleteAutopartByIdAsync(int id);

        IEnumerable<KeyValuePair<string, string>> GetAllAutopartCategoriesAsKeyValuePairs();

        IEnumerable<KeyValuePair<string, string>> GetAllAutopartConditionsAsKeyValuePairs();

        bool IsUserAutopartOwner(string userId, int autopartId);

        Task IncreaseAutopartViewsCount(int id);

        IEnumerable<T> GetAutopartsByFilters<T>(SearchFiltersDTO searchFiltersDTO);
    }
}
