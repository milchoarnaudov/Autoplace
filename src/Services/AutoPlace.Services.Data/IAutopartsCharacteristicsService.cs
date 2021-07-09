namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;

    using AutoPlace.Services.Common;
    using AutoPlace.Services.Data.Models.AutopartCharacteristics;

    public interface IAutopartsCharacteristicsService : ITransientService
    {
        IEnumerable<AutopartCharacteristic> GetAllAutopartCategories();

        IEnumerable<AutopartCharacteristic> GetAllAutopartConditions();
    }
}
