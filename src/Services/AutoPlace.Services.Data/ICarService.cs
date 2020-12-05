namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;

    public interface ICarService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllCarModelsAsKeyValuePairsById(int id);

        IEnumerable<KeyValuePair<string, string>> GetAllCarTypesAsKeyValuePairs();

        IEnumerable<KeyValuePair<string, string>> GetAllCarManufacturersAsKeyValuePairs();
    }
}
