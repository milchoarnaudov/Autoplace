namespace AutoPlace.Web.Areas.Administration.Controllers
{
    using AutoPlace.Common;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Route("api/administration/[controller]")]
    public class AdministrationController : ControllerBase
    {
    }
}
