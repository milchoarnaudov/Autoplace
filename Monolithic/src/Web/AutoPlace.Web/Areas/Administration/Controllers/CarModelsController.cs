﻿namespace AutoPlace.Web.Areas.Administration.Controllers
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
    public class CarModelsController : AdministrationController
    {
        private readonly IItemsService<CarModel> carModelsService;
        private readonly IHtmlSanitizer htmlSanitizer;

        public CarModelsController(IItemsService<CarModel> carModelsService, IHtmlSanitizer htmlSanitizer)
        {
            this.carModelsService = carModelsService;
            this.htmlSanitizer = htmlSanitizer;
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
        public async Task<IActionResult> Add([FromBody] ItemInputModel item)
        {
            var modelId = await this.carModelsService.Create(this.htmlSanitizer.Sanitize(item.Name));

            if (modelId == default)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}
