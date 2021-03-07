namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFavoritesService
    {
        Task AddToFavoriteAsync(string userId, int autopartId);

        IEnumerable<T> GetAllFavoritesAutopartByUserId<T>(string userId);

        bool CheckIfAutopartIsFavoriteForUser(string userId, int autopartId);
    }
}
