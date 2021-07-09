namespace AutoPlace.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.Models.Votes;
    using AutoPlace.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;
        private readonly IUsersService usersService;

        public VotesController(
            IVotesService votesService,
            IUsersService usersService)
        {
            this.votesService = votesService;
            this.usersService = usersService;
        }

        [HttpGet]
        public IEnumerable<VotesViewModel> All(string username)
        {
            var forUserId = this.usersService.GetUserIdByUsername(username);
            return this.votesService.GetAllByUserId<VotesViewModel>(forUserId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateVoteInputModel input)
        {
            var voterId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var forUserId = this.usersService.GetUserIdByUsername(input.ForUserUserName);

            var vote = new CreateVote
            {
                ForUserId = forUserId,
                VoterId = voterId,
                VoteValue = input.VoteValue,
            };

            await this.votesService.CreateAsync(vote);

            return this.Ok();
        }
    }
}
