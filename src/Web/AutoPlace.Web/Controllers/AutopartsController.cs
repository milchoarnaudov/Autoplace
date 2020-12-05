namespace AutoPlace.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.DTO;
    using AutoPlace.Web.ViewModels.Autoparts;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class AutopartsController : Controller
    {
        private readonly IAutopartsService autopartsService;
        private readonly ICarService carService;
        private readonly IWebHostEnvironment env;

        public AutopartsController(
            IAutopartsService autopartsService,
            ICarService carService,
            IWebHostEnvironment env)
        {
            this.autopartsService = autopartsService;
            this.carService = carService;
            this.env = env;
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
        public async Task<IActionResult> Add(CreateAutopartInputModel input)
        {
            var autopart = new CreateAutopartDTO
            {
                Name = input.Name,
                Price = input.Price,
                Description = input.Description,
                CarManufacturerId = input.CarManufacturerId,
                ModelId = input.ModelId,
                CarTypeId = input.CarTypeId,
                CategoryId = input.CategoryId,
                ConditionId = input.ConditionId,
                MakeDate = input.MakeDate,
                Images = input.Images,
            };

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var imagePath = $"{this.env.WebRootPath}/Images";

            await this.autopartsService.CreateAutopartAsync(autopart, userId, imagePath);

            return this.Redirect("/");
        }

        public IActionResult GetModelsById(int id)
        {
            var modelsById = this.carService.GetAllCarModelsAsKeyValuePairsById(id);

            return this.Json(modelsById);
        }
    }
}
