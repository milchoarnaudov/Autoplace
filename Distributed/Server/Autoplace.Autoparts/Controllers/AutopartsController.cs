using Autoplace.Autoparts.Models.InputModels;
using Autoplace.Autoparts.Models.OutputModels;
using Autoplace.Autoparts.Services;
using Autoplace.Common;
using Autoplace.Common.Controllers;
using Autoplace.Common.Services;
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
            var currentUserId = currentUserService.UserId;
            var imagePath = $"{env.WebRootPath}/Images";
            var result = await autopartsService.CreateAsync(input, currentUserId, imagePath);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AutopartOutputModel>> Get(int id)
        {
            var result = await autopartsService.GetById(id);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            await autopartsService.IncreaseViewsCountAsync(id);

            return Ok(result.Model);
        }

        [HttpGet]
        public ActionResult<AutopartOutputModel> Search([FromQuery] SearchFiltersInputModel input)
        {
            var result = autopartsService.Search(input);

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<BaseAutopartOutputModel>> Edit([FromForm] EditAutopartInputModel input)
        {
            var userId = currentUserService.UserId;
            var imagePath = $"{env.WebRootPath}/Images";

            if (!(await autopartsService.CheckIfUserIsOwnerAsync(userId, input.Id)))
            {
                return Forbid();
            }

            var result = await autopartsService.EditAsync(input, imagePath);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AutopartOutputModel>> Delete(int id)
        {
            var userId = currentUserService.UserId;

            if (!(await autopartsService.CheckIfUserIsOwnerAsync(userId, id)))
            {
                return Forbid();
            }

            var result = await autopartsService.DeleteAsync(id);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Model);
        }


        [Authorize(Roles = SystemConstants.AdministratorRoleName)]
        [HttpPost("approvals/{id}")]
        public async Task<ActionResult<BaseAutopartOutputModel>> Approve(int id)
        {
            var result = await autopartsService.MarkAsApproved(id);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Model);
        }

        [Authorize(Roles = SystemConstants.AdministratorRoleName)]
        [HttpGet("approvals")]
        public IEnumerable<AutopartOutputModel> GetForApproval(int? itemsPerPage, int? page)
            => autopartsService.GetAllForApproval(itemsPerPage, page);


    }
}
