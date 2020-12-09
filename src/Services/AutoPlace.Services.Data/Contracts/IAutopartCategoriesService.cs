namespace AutoPlace.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAutopartCategoriesService
    {
        IEnumerable<KeyValuePair<int, string>> GetAllAsKeyValuePairs();

        Task<bool> Delete(int id);

        Task<bool> Add(string name);
    }
}
