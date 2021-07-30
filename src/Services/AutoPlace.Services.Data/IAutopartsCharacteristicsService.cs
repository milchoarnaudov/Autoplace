namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;

    using AutoPlace.Services.Data.Models.AutopartCharacteristics;

    public interface IAutopartsCharacteristicsService
    {
        IEnumerable<AutopartCharacteristic> GetAllAutopartCategories();

        IEnumerable<AutopartCharacteristic> GetAllAutopartConditions();
    }
}
