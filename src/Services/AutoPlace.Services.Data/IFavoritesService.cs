namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFavoritesService
    {
        Task AddToFavorite(string userId, int autopartId);

        IEnumerable<T> GetAllFavoritesAutopartByUserId<T>(string userId);
    }
}
