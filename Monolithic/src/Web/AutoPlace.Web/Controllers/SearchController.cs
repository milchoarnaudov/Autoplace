﻿namespace AutoPlace.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.Models.Autoparts;
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
                CarManufacturers = this.carsService.GetAllManufacturersAsKeyValuePairs(),
                CarTypes = this.carsService.GetAllTypesAsKeyValuePairs(),
                Categories = this.autopartsCharacteristicsService.GetAllCategoriesAsKeyValuePairs(),
                Conditions = this.autopartsCharacteristicsService.GetAllConditionsAsKeyValuePairs(),
            };

            return this.View(viewModel);
        }

        public IActionResult Result(SearchFiltersInputModel input)
        {
            var searchFilters = new SearchFilters
            {
                MaxPrice = input.MaxPrice,
                ConditionId = input.ConditionId,
                CategoryId = input.CategoryId,
                CarManufacturerId = input.CarManufacturerId,
                ModelId = input.ModelId,
                CarTypeId = input.CarTypeId,
                CarMakeYear = input.CarMakeYear,
            };

            var viewModel = this.autopartsService.GetAll<AutopartsListItemViewModel>(searchFilters);
            return this.View(viewModel);
        }
    }
}
