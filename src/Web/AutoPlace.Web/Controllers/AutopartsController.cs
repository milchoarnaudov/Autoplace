namespace AutoPlace.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.DTO.Autoparts;
    using AutoPlace.Web.ViewModels.Autoparts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AutopartsController : Controller
    {
        private readonly IAutopartsService autopartsService;
        private readonly ICarsService carsService;
        private readonly IWebHostEnvironment env;
        private readonly IFavoritesService favoritesService;

        public AutopartsController(
            IAutopartsService autopartsService,
            ICarsService carsService,
            IWebHostEnvironment env,
            IFavoritesService favoritesService)
        {
            this.autopartsService = autopartsService;
            this.carsService = carsService;
            this.env = env;
            this.favoritesService = favoritesService;
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
        [ValidateAntiForgeryToken]
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
                MakeYear = input.CarMakeYear,
                Images = input.Images,
            };

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var imagePath = $"{this.env.WebRootPath}/Images";

            await this.autopartsService.CreateAsync(autopart, userId, imagePath);

            return this.Redirect("/");
        }

        [AllowAnonymous]
        public IActionResult GetModelsById(int id)
        {
            var modelsById = this.carsService.GetAllCarModelsAsKeyValuePairsById(id);

            return this.Json(modelsById);
        }

        [AllowAnonymous]
        public IActionResult All()
        {
            var viewModels = this.autopartsService.GetAll<AutopartsListItemViewModel>();

            return this.View(viewModels);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var viewModel = this.autopartsService.GetById<AutopartDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            await this.autopartsService.IncreaseCount(viewModel.Id);

            return this.View(viewModel);
        }

        public IActionResult Delete(int id)
        {
            var viewModel = this.autopartsService.GetById<AutopartDetailsViewModel>(id);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (viewModel == null)
            {
                return this.NotFound();
            }

            if (!this.autopartsService.IsUserAutopartOwner(userId, id))
            {
                return this.Forbid();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.autopartsService.IsUserAutopartOwner(userId, id))
            {
                return this.Forbid();
            }

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
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (viewModel == null)
            {
                return this.NotFound();
            }

            if (!this.autopartsService.IsUserAutopartOwner(userId, id))
            {
                return this.Forbid();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,Price,Description")] AutopartDetailsViewModel autopart)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.autopartsService.IsUserAutopartOwner(userId, autopart.Id))
            {
                return this.Forbid();
            }

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

        public IActionResult Favorites()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var viewModels = this.favoritesService.GetAllFavoritesAutopartByUserId<AutopartsListItemViewModel>(userId);

            return this.View(viewModels);
        }
    }
}
