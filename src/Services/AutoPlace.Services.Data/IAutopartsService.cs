namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.Models.Autoparts;

    public interface IAutopartsService
    {
        Task<int> CreateAsync(CreateAutopart autopart, string userId, string imagePath);

        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage);

        IEnumerable<T> GetAll<T>(SearchFilters searchFilters);

        Task<bool> EditAsync(EditAutopart autopart);

        Task<bool> DeleteByIdAsync(int id);

        bool CheckIfUserIsOwner(string userId, int autopartId);

        Task IncreaseViewsCountAsync(int id);

        int GetCount();
    }
}
