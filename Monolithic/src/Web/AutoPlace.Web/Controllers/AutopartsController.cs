﻿namespace AutoPlace.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoPlace.Common;
    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.Models.Autoparts;
    using AutoPlace.Web.ViewModels.Autoparts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AutopartsController : BaseController
    {
        private readonly IAutopartsService autopartsService;
        private readonly ICarsService carsService;
        private readonly IAutopartsCharacteristicsService autopartsCharacteristicsService;
        private readonly IWebHostEnvironment env;
        private readonly IFavoritesService favoritesService;

        public AutopartsController(
            IAutopartsService autopartsService,
            ICarsService carsService,
            IAutopartsCharacteristicsService autopartsCharacteristicsService,
            IWebHostEnvironment env,
            IFavoritesService favoritesService)
        {
            this.autopartsService = autopartsService;
            this.carsService = carsService;
            this.autopartsCharacteristicsService = autopartsCharacteristicsService;
            this.env = env;
            this.favoritesService = favoritesService;
        }

        public IActionResult Add()
        {
            var viewModel = new CreateAutopartInputModel
            {
                CarManufacturers = this.carsService.GetAllManufacturersAsKeyValuePairs(),
                CarTypes = this.carsService.GetAllTypesAsKeyValuePairs(),
                Categories = this.autopartsCharacteristicsService.GetAllCategoriesAsKeyValuePairs(),
                Conditions = this.autopartsCharacteristicsService.GetAllConditionsAsKeyValuePairs(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateAutopartInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CarManufacturers = this.carsService.GetAllManufacturersAsKeyValuePairs();
                input.CarTypes = this.carsService.GetAllTypesAsKeyValuePairs();
                input.Categories = this.autopartsCharacteristicsService.GetAllCategoriesAsKeyValuePairs();
                input.Conditions = this.autopartsCharacteristicsService.GetAllConditionsAsKeyValuePairs();

                return this.View(input);
            }

            var autopart = new CreateAutopart
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
            var modelsById = this.carsService.GetAllModelsAsKeyValuePairsById(id);

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
                AutopartsCount = this.autopartsService.GetCount(),
                Autoparts = this.autopartsService.GetAll<AutopartsListItemViewModel>(page, GlobalConstants.ItemsCountPerPage),
                ItemsPerPage = GlobalConstants.ItemsCountPerPage,
                PageNumber = page,
            };

            return this.View(autopartsListViewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var autopartViewModel = this.autopartsService.GetById<AutopartDetailsViewModel>(id);

            if (autopartViewModel == null)
            {
                return this.NotFound();
            }

            await this.autopartsService.IncreaseViewsCountAsync(autopartViewModel.Id);

            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                autopartViewModel.IsInFavorites = this.favoritesService.CheckIfAutopartIsFavoriteForUser(userId, autopartViewModel.Id);
            }

            return this.View(autopartViewModel);
        }

        public IActionResult Delete(int id)
        {
            var autopartViewModel = this.autopartsService.GetById<AutopartDetailsViewModel>(id);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (autopartViewModel == null)
            {
                return this.NotFound();
            }

            if (!this.autopartsService.CheckIfUserIsOwner(userId, id))
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

            if (!this.autopartsService.CheckIfUserIsOwner(userId, id))
            {
                return this.Forbid();
            }

            var isSuccessful = await this.autopartsService.DeleteByIdAsync(id);

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

            if (!this.autopartsService.CheckIfUserIsOwner(userId, id))
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
                var viewModel = this.autopartsService.GetById<AutopartDetailsViewModel>(input.Id);

                if (viewModel == null)
                {
                    return this.NotFound();
                }

                return this.View(input);
            }

            if (!this.autopartsService.CheckIfUserIsOwner(userId, input.Id))
            {
                return this.Forbid();
            }

            var isSuccessful = await this.autopartsService.EditAsync(new EditAutopart
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
