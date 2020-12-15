namespace AutoPlace.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoPlace.Services.Data;
    using AutoPlace.Services.Data.DTO.Votes;
    using AutoPlace.Web.ViewModels.Users;
    using AutoPlace.Web.ViewModels.Votes;
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

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateVoteInputModel vote)
        {
            var voterId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var forUser = this.usersService.GetByUsername<UsersListItemViewModel>(vote.ForUserUserName);

            var voteDTO = new CreateVoteDTO
            {
                ForUserId = forUser.Id,
                VoterId = voterId,
                VoteValue = vote.VoteValue,
            };

            await this.votesService.AddVote(voteDTO);

            return this.Ok();
        }
    }
}
