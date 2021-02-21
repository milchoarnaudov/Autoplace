namespace AutoPlace.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoPlace.Common;
    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.DTO.Autoparts;
    using AutoPlace.Web.ViewModels.Autoparts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AutopartsController : BaseController
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
                Categories = this.autopartsService.GetAllAutopartCategoriesAsKeyValuePairs(),
                Conditions = this.autopartsService.GetAllAutopartConditionsAsKeyValuePairs(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateAutopartInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CarManufacturers = this.carsService.GetAllCarManufacturersAsKeyValuePairs();
                input.CarTypes = this.carsService.GetAllCarTypesAsKeyValuePairs();
                input.Categories = this.autopartsService.GetAllAutopartCategoriesAsKeyValuePairs();
                input.Conditions = this.autopartsService.GetAllAutopartConditionsAsKeyValuePairs();

                return this.View(input);
            }

            var autopartDTO = new CreateAutopartDTO
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

            await this.autopartsService.CreateAutopartAsync(autopartDTO, userId, imagePath);
            return this.Redirect("/");
        }

        [AllowAnonymous]
        public IActionResult GetModelsById(int id)
        {
            var modelsById = this.carsService.GetAllCarModelsAsKeyValuePairsById(id);

            return this.Json(modelsById);
        }

        [AllowAnonymous]
        public IActionResult All(int page)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var autopartsListViewModel = new AutopartsListViewModel
            {
                AutopartsCount = this.autopartsService.GetAutopartsCount(),
                Autoparts = this.autopartsService.GetAllAutoparts<AutopartsListItemViewModel>(page, GlobalConstants.ItemsCountPerPage),
                ItemsPerPage = GlobalConstants.ItemsCountPerPage,
                PageNumber = page,
            };

            return this.View(autopartsListViewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var autopartViewModel = this.autopartsService.GetAutopartById<AutopartDetailsViewModel>(id);

            if (autopartViewModel == null)
            {
                return this.NotFound();
            }

            await this.autopartsService.IncreaseAutopartViewsCount(autopartViewModel.Id);

            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                autopartViewModel.IsInFavorites = this.favoritesService.IsAutopartFavoriteForUser(userId, autopartViewModel.Id);
            }

            return this.View(autopartViewModel);
        }

        public IActionResult Delete(int id)
        {
            var autopartViewModel = this.autopartsService.GetAutopartById<AutopartDetailsViewModel>(id);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (autopartViewModel == null)
            {
                return this.NotFound();
            }

            if (!this.autopartsService.IsUserAutopartOwner(userId, id))
            {
                return this.Forbid();
            }

            return this.View(autopartViewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDeletion(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.autopartsService.IsUserAutopartOwner(userId, id))
            {
                return this.Forbid();
            }

            var isSuccessful = await this.autopartsService.DeleteAutopartByIdAsync(id);

            if (!isSuccessful)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("All");
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.autopartsService.GetAutopartById<AutopartDetailsViewModel>(id);
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
        public async Task<IActionResult> Edit([Bind("Id,Name,Price,Description")] AutopartDetailsViewModel input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.ModelState.IsValid)
            {
                var viewModel = this.autopartsService.GetAutopartById<AutopartDetailsViewModel>(input.Id);

                if (viewModel == null)
                {
                    return this.NotFound();
                }

                return this.View(input);
            }

            if (!this.autopartsService.IsUserAutopartOwner(userId, input.Id))
            {
                return this.Forbid();
            }

            var isSuccessful = await this.autopartsService.EditAutopart(new EditAutopartDTO
            {
                Id = input.Id,
                Description = input.Description,
                Name = input.Name,
                Price = input.Price,
            });

            if (!isSuccessful)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("All");
        }
    }
}
