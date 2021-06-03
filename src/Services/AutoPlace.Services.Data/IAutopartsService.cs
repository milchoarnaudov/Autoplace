namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Common;
    using AutoPlace.Services.Data.DTO.Autoparts;

    public interface IAutopartsService : ITransientService
    {
        Task CreateAsync(CreateAutopartDTO autopart, string userId, string imagePath);

        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage);

        IEnumerable<T> GetAll<T>(SearchFiltersDTO searchFiltersDTO);

        Task<bool> EditAsync(EditAutopartDTO autopart);

        Task<bool> DeleteByIdAsync(int id);

        bool CheckIfUserIsOwner(string userId, int autopartId);

        Task IncreaseViewsCountAsync(int id);

        int GetCount();
    }
}
