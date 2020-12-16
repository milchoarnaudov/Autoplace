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

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Route("api/administration/[controller]")]
    [ApiController]
    public class AutopartCategoriesController : ControllerBase
    {
        private readonly IItemsService<AutopartCategory> autopartCategoriesService;
        private readonly IHtmlSanitizer htmlSanitizer;

        public AutopartCategoriesController(IItemsService<AutopartCategory> autopartCategoriesService, IHtmlSanitizer htmlSanitizer)
        {
            this.autopartCategoriesService = autopartCategoriesService;
            this.htmlSanitizer = htmlSanitizer;
        }

        [HttpGet]
        public ActionResult<IEnumerable<KeyValuePair<int, string>>> All()
        {
            return this.autopartCategoriesService.GetAllAsKeyValuePairs().ToList();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccessful = await this.autopartCategoriesService.Delete(id);

            if (isSuccessful)
            {
                return this.Ok();
            }

            return this.NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] NameInputModel item)
        {
            var isSuccessful = await this.autopartCategoriesService.Add(this.htmlSanitizer.Sanitize(item.Name));

            if (isSuccessful)
            {
                return this.Ok();
            }

            return this.BadRequest();
        }
    }
}
