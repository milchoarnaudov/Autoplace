namespace AutoPlace.Web.Areas.Administration.Controllers
{
    using AutoPlace.Web.Controllers;
    using Microsoft.AspNetCore.Mvc;

    public class ConfigurationController : AdministrationController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
