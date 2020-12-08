namespace AutoPlace.Web.Controllers
{
    using AutoPlace.Services.Data;
    using AutoPlace.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult All()
        {
            var viewModel = new UsersListViewModel
            {
                Users = this.userService.GetAll<UsersListItemViewModel>(),
            };

            return this.View(viewModel);
        }

        public IActionResult Details(string username)
        {
            var viewModel = this.userService.GetByUsername<UserDetailsViewModel>(username);

            return this.View(viewModel);
        }
    }
}
