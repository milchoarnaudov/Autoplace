namespace AutoPlace.Web.Controllers
{
    using AutoPlace.Services.Data;
    using AutoPlace.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Mvc;

    // TODO
    public class UsersController : BaseController
    {
        private readonly IUserService usersService;

        public UsersController(IUserService usersService)
        {
            this.usersService = usersService;
        }

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

            return this.View(viewModel);
        }
    }
}
