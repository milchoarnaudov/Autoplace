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
    public class AutopartConditionsController : AdministrationController
    {
        private readonly IItemsService<AutopartCondition> autopartConditionService;
        private readonly IHtmlSanitizer htmlSanitizer;

        public AutopartConditionsController(IItemsService<AutopartCondition> autopartConditionService, IHtmlSanitizer htmlSanitizer)
        {
            this.autopartConditionService = autopartConditionService;
            this.htmlSanitizer = htmlSanitizer;
        }

        [HttpGet]
        public ActionResult<IEnumerable<KeyValuePair<int, string>>> All()
        {
            return this.autopartConditionService.GetAllAsKeyValuePairs().ToList();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccessful = await this.autopartConditionService.Delete(id);

            if (isSuccessful)
            {
                return this.Ok();
            }

            return this.NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ItemInputModel item)
        {
            var conditionId = await this.autopartConditionService.Create(this.htmlSanitizer.Sanitize(item.Name));

            if (conditionId == default)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}
