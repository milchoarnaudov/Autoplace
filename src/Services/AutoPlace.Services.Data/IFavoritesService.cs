namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Common;

    public interface IFavoritesService : ITransientService
    {
        Task AddToFavoriteAsync(string userId, int autopartId);

        IEnumerable<T> GetAllFavoritesAutopartByUserId<T>(string userId);

        bool CheckIfAutopartIsFavoriteForUser(string userId, int autopartId);
    }
}
