namespace AutoPlace.Services.Data
{
    using System.Collections.Generic;

    using AutoPlace.Services.Data.DTO.AutopartCharacteristics;

    public interface IAutopartsCharacteristicsService
    {
        IEnumerable<AutopartCharacteristic> GetAllAutopartCategories();

        IEnumerable<AutopartCharacteristic> GetAllAutopartConditions();
    }
}
