using Autoplace.Autoparts.Common;
using Autoplace.Autoparts.Models.InputModels;
using Autoplace.Autoparts.Models.OutputModels;
using Autoplace.Autoparts.Services;
using Autoplace.Common;
using Autoplace.Common.Controllers;
using Autoplace.Common.Errors;
using Autoplace.Common.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autoplace.Autoparts.Controllers
{
    [Authorize]
    public class AutopartsController : BaseApiController
    {
        private readonly IAutopartsService autopartsService;
        private readonly ICurrentUserService currentUserService;
        private readonly IWebHostEnvironment env;

        public AutopartsController(
            IAutopartsService autopartsService,
            ICurrentUserService currentUserService,
            IWebHostEnvironment env)
        {
            this.autopartsService = autopartsService;
            this.currentUserService = currentUserService;
            this.env = env;
        }

        [HttpPost]
        public async Task<ActionResult<AutopartOutputModel>> Create([FromForm] CreateAutopartInputModel input)
        {
            var username = currentUserService.Username;
            var imagePath = $"{env.WebRootPath}/Images";
            var result = await autopartsService.CreateAsync(input, username, imagePath);

            if (!result.IsSuccessful)
            {
                return BadRequest(ApiResponse.Failure(result.ErrorMessages));
            }

            return Ok(result.Model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetailedAutopartOutputModel>> Get(string id)
        {
            var autopart = await autopartsService.GetAsync(id);

            if (autopart == null)
            {
                return NotFound(ApiResponse.Failure(ErrorMessages.AutopartNotFoundErrorMessage));
            }

            await autopartsService.IncreaseViewsCountAsync(id);

            return Ok(autopart);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutopartOutputModel>>> Search([FromQuery] SearchFiltersInputModel input)
        {
            var result = await autopartsService.SearchAsync(input);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AutopartOutputModel>> Edit([FromForm] AutopartInputModel input, string id)
        {
            var username = currentUserService.Username;
            var imagePath = $"{env.WebRootPath}/Images";

            if (!(await autopartsService.CheckIfUserIsOwnerAsync(username, id)))
            {
                return BadRequest(ApiResponse.Failure(GenericErrorMessages.OperationNotAllowed));
            }

            var result = await autopartsService.EditAsync(id, input, imagePath);

            if (!result.IsSuccessful)
            {
                return BadRequest(ApiResponse.Failure(result.ErrorMessages));
            }

            return Ok(result.Model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AutopartOutputModel>> Delete(string id)
        {
            var username = currentUserService.Username;

            if (!(await autopartsService.CheckIfUserIsOwnerAsync(username, id)))
            {
                return Forbid();
            }

            var result = await autopartsService.DeleteAsync(id);

            if (!result.IsSuccessful)
            {
                return BadRequest(ApiResponse.Failure(result.ErrorMessages));
            }

            return Ok(result.Model);
        }
    }
}
