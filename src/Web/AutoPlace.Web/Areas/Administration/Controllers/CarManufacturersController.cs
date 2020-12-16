namespace AutoPlace.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Common;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.AdministrationServices.Contracts;
    using AutoPlace.Web.ViewModels.Common;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Route("api/administration/[controller]")]
    [ApiController]
    public class CarManufacturersController : ControllerBase
    {
        private readonly IItemsService<CarManufacturer> carManufacturersService;
        private readonly IHtmlSanitizer htmlSanitizer;

        public CarManufacturersController(IItemsService<CarManufacturer> carManufacturersService, IHtmlSanitizer htmlSanitizer)
        {
            this.carManufacturersService = carManufacturersService;
            this.htmlSanitizer = htmlSanitizer;
        }

        [HttpGet]
        public ActionResult<IEnumerable<KeyValuePair<int, string>>> All()
        {
            return this.carManufacturersService.GetAllAsKeyValuePairs().ToList();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccessful = await this.carManufacturersService.Delete(id);

            if (isSuccessful)
            {
                return this.Ok();
            }

            return this.NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] NameInputModel item)
        {
            var isSuccessful = await this.carManufacturersService.Add(this.htmlSanitizer.Sanitize(item.Name));

            if (isSuccessful)
            {
                return this.Ok();
            }

            return this.BadRequest();
        }
    }
}
