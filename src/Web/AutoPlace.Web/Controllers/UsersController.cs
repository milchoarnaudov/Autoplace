namespace AutoPlace.Web.Controllers
{
    using System.Security.Claims;

    using AutoPlace.Common;
    using AutoPlace.Services.Data;
    using AutoPlace.Web.ViewModels.Users;
    using AutoPlace.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Details(string username)
        {
            var userViewModel = this.usersService.GetByUsername<UserDetailsViewModel>(username);

            if (userViewModel == null)
            {
                return this.NotFound();
            }

            var voterId = this.User.Identity.IsAuthenticated ? this.User.FindFirst(ClaimTypes.NameIdentifier).Value : string.Empty;

            var currentUserVote = this.votesService.GetVote<VotesViewModel>(userViewModel.Id, voterId);

            if (currentUserVote != null)
            {
                if (currentUserVote.VoteValue)
                {
                    userViewModel.IsCurrentUserVotedPositive = true;
                }
                else
                {
                    userViewModel.IsCurrentUserVotedNegative = true;
                }
            }

            return this.View(userViewModel);
        }
    }
}
