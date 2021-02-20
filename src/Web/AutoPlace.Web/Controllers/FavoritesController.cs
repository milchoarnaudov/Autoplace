namespace AutoPlace.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data;
    using AutoPlace.Web.ViewModels.Autoparts;
    using AutoPlace.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class FavoritesController : BaseController
    {
        private readonly IFavoritesService favoritesService;

        public FavoritesController(IFavoritesService favoritesService)
        {
            this.favoritesService = favoritesService;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModels = this.favoritesService.GetAllFavoritesAutopartByUserId<AutopartsListItemViewModel>(userId);

            return this.View(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] IdInputModel autopart)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await this.favoritesService.AddToFavorite(userId, autopart.Id);
            return this.Ok();
        }
    }
}
