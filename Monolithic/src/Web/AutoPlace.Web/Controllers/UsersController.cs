namespace AutoPlace.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;

    using AutoPlace.Data.Models;
    using AutoPlace.Services.Data;
    using AutoPlace.Services.Mapping;
    using AutoPlace.Web.ViewModels.Users;
    using AutoPlace.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IVotesService votesService;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            IVotesService votesService)
        {
            this.userManager = userManager;
            this.votesService = votesService;
        }

        public IActionResult Details(string username)
        {
            var userViewModel = this.userManager.Users.Where(x => x.UserName == username)
                .To<UserDetailsViewModel>()
                .FirstOrDefault();

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
