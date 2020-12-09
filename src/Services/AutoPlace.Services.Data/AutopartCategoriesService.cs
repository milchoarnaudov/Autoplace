namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Repositories;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.Contracts;

    public class AutopartCategoriesService : IAutopartCategoriesService
    {
        private readonly IDeletableEntityRepository<AutopartCategory> autopartCategoriesService;

        public AutopartCategoriesService(IDeletableEntityRepository<AutopartCategory> autopartCategoriesService)
        {
            this.autopartCategoriesService = autopartCategoriesService;
        }

        public async Task<bool> Add(string name)
        {
            var category = this.autopartCategoriesService.All().Where(x => x.Name == name).FirstOrDefault();

            if (category != null)
            {
                return false;
            }

            category = new AutopartCategory
            {
                Name = name,
            };

            await this.autopartCategoriesService.AddAsync(category);
            await this.autopartCategoriesService.SaveChangesAsync();

            return true;
        }

        public IEnumerable<KeyValuePair<int, string>> GetAllAsKeyValuePairs()
        {
            return this.autopartCategoriesService.All().Select(x => new KeyValuePair<int, string>(x.Id, x.Name)).ToList();
        }

        public async Task<bool> Delete(int id)
        {
            var category = this.autopartCategoriesService.All().Where(x => x.Id == id).FirstOrDefault();

            if (category == null)
            {
                return false;
            }

            this.autopartCategoriesService.Delete(category);
            await this.autopartCategoriesService.SaveChangesAsync();

            return true;
        }
    }
}
