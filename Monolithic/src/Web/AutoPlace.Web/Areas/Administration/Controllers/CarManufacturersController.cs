namespace AutoPlace.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.Administration;
    using AutoPlace.Web.ViewModels.Common;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class CarManufacturersController : AdministrationController
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
        public async Task<IActionResult> Add([FromBody] ItemInputModel item)
        {
            var manufacturerId = await this.carManufacturersService.Create(this.htmlSanitizer.Sanitize(item.Name));

            if (manufacturerId == default)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}
