namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Mapping;

    public class FavoritesService : IFavoritesService
    {
        private readonly IDeletableEntityRepository<Favorite> favoritesRepository;

        public FavoritesService(IDeletableEntityRepository<Favorite> favoritesRepository)
        {
            this.favoritesRepository = favoritesRepository;
        }

        public async Task AdFavoriteAsync(string userId, int autopartId)
        {
            var favoriteExistingEntity = this.favoritesRepository.AllAsNoTracking().Where(x => x.UserId == userId && x.AutopartId == autopartId).FirstOrDefault();

            if (favoriteExistingEntity != null)
            {
                this.favoritesRepository.HardDelete(favoriteExistingEntity);
            }
            else
            {
                var favoriteEntity = new Favorite
                {
                    UserId = userId,
                    AutopartId = autopartId,
                };

                await this.favoritesRepository.AddAsync(favoriteEntity);
            }

            await this.favoritesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllFavoritesAutopartByUserId<T>(string userId) =>
            this.favoritesRepository.AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.Autopart)
                .To<T>();

        public bool CheckIfAutopartIsFavoriteForUser(string userId, int autopartId) => this.favoritesRepository.AllAsNoTracking().Any(x => x.UserId == userId && x.AutopartId == autopartId);
    }
}
