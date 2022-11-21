using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoplace.Common.Controllers
{
    [Authorize(Roles = SystemConstants.AdministratorRoleName)]
    public abstract class BaseAdminApiController : BaseApiController
    {
    }
}
