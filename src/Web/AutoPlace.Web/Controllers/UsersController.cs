namespace AutoPlace.Web.Controllers
{
    using AutoPlace.Common;
    using AutoPlace.Services.Data;
    using AutoPlace.Web.ViewModels.Users;
    using AutoPlace.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IVotesService votesService;

        public UsersController(
            IUsersService usersService,
            IVotesService votesService)
        {
            this.usersService = usersService;
            this.votesService = votesService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult All()
        {
            var viewModel = new UsersListViewModel
            {
                Users = this.usersService.GetAll<UsersListItemViewModel>(),
            };

            return this.View(viewModel);
        }

        public IActionResult Details(string username)
        {
            var viewModel = this.usersService.GetByUsername<UserDetailsViewModel>(username);
            var forUserId = this.usersService.GetUserIdByUsername(username);
            var voterId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var currentUserVote = this.votesService.GetVote<VotesViewModel>(forUserId, voterId);

            if (currentUserVote != null)
            {
                if (currentUserVote.VoteValue)
                {
                    viewModel.IsCurrentUserVotedPositive = true;
                }
                else
                {
                    viewModel.IsCurrentUserVotedNegative = true;
                }
            }

            return this.View(viewModel);
        }
    }
}
