namespace AutoPlace.Web.Areas.Administration.Controllers
{
    using AutoPlace.Common;
    using AutoPlace.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class ConfigurationController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
