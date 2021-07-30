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

        public async Task<int> AddFavoriteAsync(string userId, int autopartId, bool toDeleteIfExists = true)
        {
            if (userId is null || autopartId == default)
            {
                return 0;
            }

            var favoriteExistingEntity = this.favoritesRepository.All()
                .Where(x => x.UserId == userId && x.AutopartId == autopartId)
                .FirstOrDefault();

            if (favoriteExistingEntity != null)
            {
                if (toDeleteIfExists)
                {
                    this.favoritesRepository.HardDelete(favoriteExistingEntity);
                    await this.favoritesRepository.SaveChangesAsync();
                }

                return favoriteExistingEntity.Id;
            }

            var favoriteEntity = new Favorite
            {
                UserId = userId,
                AutopartId = autopartId,
            };

            await this.favoritesRepository.AddAsync(favoriteEntity);
            await this.favoritesRepository.SaveChangesAsync();

            return favoriteEntity.Id;
        }

        public IEnumerable<T> GetAllFavoritesAutopartByUserId<T>(string userId) =>
            this.favoritesRepository.AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.Autopart)
                .To<T>();

        public bool CheckIfAutopartIsFavoriteForUser(string userId, int autopartId) =>
            this.favoritesRepository.All()
                .Any(x => x.UserId == userId && x.AutopartId == autopartId);
    }
}
