namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.DTO.Autoparts;

    public interface IAutopartsService
    {
        Task CreateAsync(CreateAutopartDTO autopart, string userId, string imagePath);

        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage);

        IEnumerable<T> GetAll<T>(SearchFiltersDTO searchFiltersDTO);

        Task<bool> EditAsync(EditAutopartDTO autopart);

        Task<bool> DeleteByIdAsync(int id);

        IEnumerable<KeyValuePair<string, string>> GetAllAutopartCategoriesAsKeyValuePairs();

        IEnumerable<KeyValuePair<string, string>> GetAllAutopartConditionsAsKeyValuePairs();

        bool CheckIfUserIsOwner(string userId, int autopartId);

        Task IncreaseViewsCountAsync(int id);

        int GetCount();
    }
}
