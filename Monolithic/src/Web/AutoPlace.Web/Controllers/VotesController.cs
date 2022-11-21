namespace AutoPlace.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.Models.Votes;
    using AutoPlace.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;
        private readonly UserManager<ApplicationUser> userManager;

        public VotesController(
            IVotesService votesService,
            UserManager<ApplicationUser> userManager)
        {
            this.votesService = votesService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<VotesViewModel>> All(string username)
        {
            var forUser = await this.userManager.FindByNameAsync(username);

            return this.votesService.GetAllByUserId<VotesViewModel>(forUser.Id);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateVoteInputModel input)
        {
            var voterId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var forUser = await this.userManager.FindByNameAsync(input.ForUserUserName);

            var vote = new CreateVote
            {
                ForUserId = forUser.Id,
                VoterId = voterId,
                VoteValue = input.VoteValue,
            };

            await this.votesService.CreateAsync(vote);

            return this.Ok();
        }
    }
}
