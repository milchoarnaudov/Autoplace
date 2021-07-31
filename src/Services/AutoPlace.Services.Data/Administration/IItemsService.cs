namespace AutoPlace.Services.Data.Administration
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Data.Common.Models;

    public interface IItemsService<TEntity>
        where TEntity : class, IDeletableEntity, IItemEntity, new()
    {
        Task<int> Create(string name);

        IEnumerable<KeyValuePair<int, string>> GetAllAsKeyValuePairs();

        Task<bool> Delete(int id);
    }
}
