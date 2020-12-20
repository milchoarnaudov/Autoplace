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

        public async Task AddToFavorite(string userId, int autopartId)
        {
            var alreadyAdded = this.favoritesRepository.AllAsNoTracking().Where(x => x.UserId == userId && x.AutopartId == autopartId).FirstOrDefault();

            if (alreadyAdded != null)
            {
                this.favoritesRepository.HardDelete(alreadyAdded);
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
    }
}
