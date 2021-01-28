namespace AutoPlace.Services.Data.Administration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Models;
    using AutoPlace.Data.Common.Repositories;

    public class ItemsService<TEntity> : IItemsService<TEntity>
        where TEntity : class, IDeletableEntity, IItemEntity, new()
    {
        private readonly IDeletableEntityRepository<TEntity> itemsRepository;

        public ItemsService(IDeletableEntityRepository<TEntity> itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        public async Task<bool> Add(string name)
        {
            var category = this.itemsRepository.All().Where(x => x.Name == name).FirstOrDefault();

            if (category != null)
            {
                return false;
            }

            category = new TEntity
            {
                Name = name,
            };

            await this.itemsRepository.AddAsync(category);
            await this.itemsRepository.SaveChangesAsync();

            return true;
        }

        public IEnumerable<KeyValuePair<int, string>> GetAllAsKeyValuePairs()
        {
            return this.itemsRepository.All().Select(x => new KeyValuePair<int, string>(x.Id, x.Name)).ToList();
        }

        public async Task<bool> Delete(int id)
        {
            var category = this.itemsRepository.All().Where(x => x.Id == id).FirstOrDefault();

            if (category == null)
            {
                return false;
            }

            this.itemsRepository.Delete(category);
            await this.itemsRepository.SaveChangesAsync();

            return true;
        }
    }
}
