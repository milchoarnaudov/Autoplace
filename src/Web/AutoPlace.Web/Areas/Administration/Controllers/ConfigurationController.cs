namespace AutoPlace.Web.Areas.Administration.Controllers
{
    using AutoPlace.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class ConfigurationController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
