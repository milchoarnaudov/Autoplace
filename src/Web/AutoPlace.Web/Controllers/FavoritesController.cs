namespace AutoPlace.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data;
    using AutoPlace.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoritesService favoritesService;

        public FavoritesController(IFavoritesService favoritesService)
        {
            this.favoritesService = favoritesService;
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
