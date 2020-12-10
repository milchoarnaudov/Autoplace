namespace AutoPlace.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.DTO;
    using AutoPlace.Services.Data.DTO.Autoparts;
    using AutoPlace.Web.ViewModels.Autoparts;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    // TODO 
    public class AutopartsController : Controller
    {
        private readonly IAutopartsService autopartsService;
        private readonly ICarService carsService;
        private readonly IWebHostEnvironment env;

        public AutopartsController(
            IAutopartsService autopartsService,
            ICarService carsService,
            IWebHostEnvironment env)
        {
            this.autopartsService = autopartsService;
            this.carsService = carsService;
            this.env = env;
        }

        public IActionResult Add()
        {
            var viewModel = new CreateAutopartInputModel
            {
                CarManufacturers = this.carsService.GetAllCarManufacturersAsKeyValuePairs(),
                CarTypes = this.carsService.GetAllCarTypesAsKeyValuePairs(),
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

            await this.autopartsService.CreateAsync(autopart, userId, imagePath);

            return this.Redirect("/");
        }

        public IActionResult GetModelsById(int id)
        {
            var modelsById = this.carsService.GetAllCarModelsAsKeyValuePairsById(id);

            return this.Json(modelsById);
        }

        public IActionResult All()
        {
            var viewModel = new AutopartsListViewModel
            {
                Autoparts = this.autopartsService.GetAll<AutopartsListItemViewModel>(),
            };

            return this.View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var viewModel = this.autopartsService.GetById<AutopartDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public IActionResult Delete(int id)
        {
            var viewModel = this.autopartsService.GetById<AutopartDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isSuccessful = await this.autopartsService.DeleteById(id);

            if (!isSuccessful)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("All");
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.autopartsService.GetById<AutopartDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,Price,Description")] AutopartDetailsViewModel autopart)
        {
            var isSuccessful = await this.autopartsService.Edit(new EditAutopartDTO
            {
                Id = autopart.Id,
                Description = autopart.Description,
                Name = autopart.Name,
                Price = autopart.Price,
            });

            if (!isSuccessful)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("All");
        }
    }
}
