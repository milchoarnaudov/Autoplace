namespace AutoPlace.Web.Areas.Administration.Controllers
{
    using AutoPlace.Common;
    using AutoPlace.Services.Data;
    using AutoPlace.Web.Controllers;
    using AutoPlace.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;

        public UsersController(
            IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult All()
        {
            var viewModels = this.usersService.GetAll<UsersListItemViewModel>();

            return this.View(viewModels);
        }
    }
}
