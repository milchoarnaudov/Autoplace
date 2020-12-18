namespace AutoPlace.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.DTO.Votes;
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
        public async Task<IActionResult> Add([FromBody] CreateVoteInputModel vote)
        {
            var voterId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var forUserId = this.usersService.GetUserIdByUsername(vote.ForUserUserName);

            var voteDTO = new CreateVoteDTO
            {
                ForUserId = forUserId,
                VoterId = voterId,
                VoteValue = vote.VoteValue,
            };

            await this.votesService.AddVote(voteDTO);

            return this.Ok();
        }
    }
}
