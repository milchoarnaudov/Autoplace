namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;

    using AutoPlace.Services.Common;

    public interface ICarsService : ITransientService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllCarModelsAsKeyValuePairsById(int id);

        IEnumerable<KeyValuePair<string, string>> GetAllCarTypesAsKeyValuePairs();

        IEnumerable<KeyValuePair<string, string>> GetAllCarManufacturersAsKeyValuePairs();
    }
}
