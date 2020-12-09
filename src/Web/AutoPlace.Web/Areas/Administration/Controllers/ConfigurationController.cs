namespace AutoPlace.Web.Areas.Administration.Controllers
{
    using AutoPlace.Web.Controllers;
    using Microsoft.AspNetCore.Mvc;

    [Area("Administration")]
    public class ConfigurationController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
