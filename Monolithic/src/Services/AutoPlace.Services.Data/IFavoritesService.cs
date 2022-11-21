namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFavoritesService
    {
        Task<int> AddFavoriteAsync(string userId, int autopartId, bool toDeleteIfExists = true);

        IEnumerable<T> GetAllFavoritesAutopartByUserId<T>(string userId);

        bool CheckIfAutopartIsFavoriteForUser(string userId, int autopartId);
    }
}
