namespace AutoPlace.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data.Contracts;
    using AutoPlace.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/administration/[controller]")]
    [ApiController]
    public class AutopartCategoriesController : ControllerBase
    {
        private readonly IAutopartCategoriesService autopartCategoryService;

        public AutopartCategoriesController(IAutopartCategoriesService autopartCategoryService)
        {
            this.autopartCategoryService = autopartCategoryService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<KeyValuePair<int, string>>> All()
        {
            return this.autopartCategoryService.GetAllAsKeyValuePairs().ToList();
        }

        [IgnoreAntiforgeryToken]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccessful = await this.autopartCategoryService.Delete(id);

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
            var isSuccessful = await this.autopartCategoryService.Add(item.Name);

            if (isSuccessful)
            {
                return this.Ok();
            }

            return this.BadRequest();
        }
    }
}
