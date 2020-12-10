namespace AutoPlace.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.AdministrationServices.Contracts;
    using AutoPlace.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/administration/[controller]")]
    [ApiController]
    public class CarTypesController : ControllerBase
    {
        private readonly IItemsService<CarType> carTypesService;

        public CarTypesController(IItemsService<CarType> carTypesService)
        {
            this.carTypesService = carTypesService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<KeyValuePair<int, string>>> All()
        {
            return this.carTypesService.GetAllAsKeyValuePairs().ToList();
        }

        [IgnoreAntiforgeryToken]
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

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ItemViewModel item)
        {
            var isSuccessful = await this.carTypesService.Add(item.Name);

            if (isSuccessful)
            {
                return this.Ok();
            }

            return this.BadRequest();
        }
    }
}
