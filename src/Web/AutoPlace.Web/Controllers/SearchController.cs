namespace AutoPlace.Web.Controllers
{
    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.DTO.Autoparts;
    using AutoPlace.Web.ViewModels.Autoparts;
    using AutoPlace.Web.ViewModels.Search;
    using Microsoft.AspNetCore.Mvc;

    public class SearchController : Controller
    {
        private readonly IAutopartsService autopartsService;
        private readonly ICarsService carsService;

        public SearchController(
            IAutopartsService autopartsService,
            ICarsService carsService)
        {
            this.autopartsService = autopartsService;
            this.carsService = carsService;
        }

        public IActionResult Index()
        {
            var viewModel = new SearchFiltersInputModel
            {
                CarManufacturers = this.carsService.GetAllCarManufacturersAsKeyValuePairs(),
                CarTypes = this.carsService.GetAllCarTypesAsKeyValuePairs(),
                Categories = this.autopartsService.GetAllAutopartCategoriesAsKeyValuePairs(),
                Conditions = this.autopartsService.GetAllAutopartConditionsAsKeyValuePairs(),
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

            var results = this.autopartsService.GetAutopartsByFilters<AutopartsListItemViewModel>(searchFiltersDTO);

            return this.View(results);
        }
    }
}
