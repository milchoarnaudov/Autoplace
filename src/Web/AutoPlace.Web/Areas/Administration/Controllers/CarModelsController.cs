﻿namespace AutoPlace.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Common;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data.AdministrationServices.Contracts;
    using AutoPlace.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Route("api/administration/[controller]")]
    [ApiController]
    public class CarModelsController : ControllerBase
    {
        private readonly IItemsService<CarModel> carModelsService;

        public CarModelsController(IItemsService<CarModel> carModelsService)
        {
            this.carModelsService = carModelsService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<KeyValuePair<int, string>>> All()
        {
            return this.carModelsService.GetAllAsKeyValuePairs().ToList();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccessful = await this.carModelsService.Delete(id);

            if (isSuccessful)
            {
                return this.Ok();
            }

            return this.NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] NameViewModel item)
        {
            var isSuccessful = await this.carModelsService.Add(item.Name);

            if (isSuccessful)
            {
                return this.Ok();
            }

            return this.BadRequest();
        }
    }
}
