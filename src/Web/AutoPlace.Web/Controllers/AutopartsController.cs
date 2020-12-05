namespace AutoPlace.Web.Controllers
{
    using AutoPlace.Services.Data;
    using AutoPlace.Web.ViewModels.Autoparts;
    using Microsoft.AspNetCore.Mvc;

    public class AutopartsController : Controller
    {
        private readonly IAutopartsService autopartsService;
        private readonly ICarService carService;

        public AutopartsController(
            IAutopartsService autopartsService,
            ICarService carService)
        {
            this.autopartsService = autopartsService;
            this.carService = carService;
        }

        public IActionResult Add()
        {
            var viewModel = new CreateAutopartInputModel
            {
                CarManufacturers = this.carService.GetAllCarManufacturersAsKeyValuePairs(),
                CarTypes = this.carService.GetAllCarTypesAsKeyValuePairs(),
                Categories = this.autopartsService.GetAllCategoriesAsKeyValuePairs(),
                Conditions = this.autopartsService.GetAllConditionsAsKeyValuePairs(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Add(CreateAutopartInputModel input)
        {
            return this.Redirect("/");
        }

        public IActionResult GetModelsById(int id)
        {
            var modelsById = this.carService.GetAllCarModelsAsKeyValuePairsById(id);

            return this.Json(modelsById);
        }
    }
}
