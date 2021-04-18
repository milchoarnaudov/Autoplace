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
    public class CarTypesController : AdministrationController
    {
        private readonly IItemsService<CarType> carTypesService;
        private readonly IHtmlSanitizer htmlSanitizer;

        public CarTypesController(IItemsService<CarType> carTypesService, IHtmlSanitizer htmlSanitizer)
        {
            this.carTypesService = carTypesService;
            this.htmlSanitizer = htmlSanitizer;
        }

        [HttpGet]
        public ActionResult<IEnumerable<KeyValuePair<int, string>>> All()
        {
            return this.carTypesService.GetAllAsKeyValuePairs().ToList();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccessful = await this.carTypesService.Delete(id);

            if (isSuccessful)
            {
                return this.Ok();
            }

            return this.NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ItemInputModel item)
        {
            var isSuccessful = await this.carTypesService.Add(this.htmlSanitizer.Sanitize(item.Name));

            if (isSuccessful)
            {
                return this.Ok();
            }

            return this.BadRequest();
        }
    }
}
