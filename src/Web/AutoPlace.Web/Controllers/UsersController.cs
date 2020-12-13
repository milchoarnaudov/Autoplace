namespace AutoPlace.Web.Controllers
{
    using AutoPlace.Common;
    using AutoPlace.Services.Data;
    using AutoPlace.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly IUserService usersService;

        public UsersController(IUserService usersService)
        {
            this.usersService = usersService;
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

            return this.View(viewModel);
        }
    }
}
