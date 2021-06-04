namespace AutoPlace.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.DTO.Autoparts;
    using AutoPlace.Web.ViewModels.Autoparts;
    using AutoPlace.Web.ViewModels.Search;
    using Microsoft.AspNetCore.Mvc;

    public class SearchController : BaseController
    {
        private readonly IAutopartsService autopartsService;
        private readonly ICarsService carsService;
        private readonly IAutopartsCharacteristicsService autopartsCharacteristicsService;

        public SearchController(
            IAutopartsService autopartsService,
            ICarsService carsService,
            IAutopartsCharacteristicsService autopartsCharacteristicsService)
        {
            this.autopartsService = autopartsService;
            this.carsService = carsService;
            this.autopartsCharacteristicsService = autopartsCharacteristicsService;
        }

        public IActionResult Index()
        {
            var viewModel = new SearchFiltersInputModel
            {
                CarManufacturers = this.carsService.GetAllCarManufacturersAsKeyValuePairs(),
                CarTypes = this.carsService.GetAllCarTypesAsKeyValuePairs(),
                Categories = this.autopartsCharacteristicsService.GetAllAutopartCategories().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Value)),
                Conditions = this.autopartsCharacteristicsService.GetAllAutopartConditions().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Value)),
            };

            return this.View(viewModel);
        }

        public IActionResult Result(SearchFiltersInputModel searchFilters)
        {
            var searchFiltersDTO = new SearchFiltersDTO
            {
                MaxPrice = searchFilters.MaxPrice,
                ConditionId = searchFilters.ConditionId,
                CategoryId = searchFilters.CategoryId,
                CarManufacturerId = searchFilters.CarManufacturerId,
                ModelId = searchFilters.ModelId,
                CarTypeId = searchFilters.CarTypeId,
                CarMakeYear = searchFilters.CarMakeYear,
            };

            var viewModel = this.autopartsService.GetAll<AutopartsListItemViewModel>(searchFiltersDTO);
            return this.View(viewModel);
        }
    }
}
