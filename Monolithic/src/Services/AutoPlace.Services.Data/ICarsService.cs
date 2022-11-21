namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;

    public interface ICarsService
    {
        IEnumerable<KeyValuePair<int, string>> GetAllModelsAsKeyValuePairsById(int id);

        IEnumerable<KeyValuePair<int, string>> GetAllTypesAsKeyValuePairs();

        IEnumerable<KeyValuePair<int, string>> GetAllManufacturersAsKeyValuePairs();
    }
}
