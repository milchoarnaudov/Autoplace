using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Autoplace.Common.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {

            this.httpContextAccessor = httpContextAccessor;
        }

        public string UserId
        {
            get
            {
                var user = GetUser();
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return userId;
            }
        }

        public bool IsAdministrator
        {
            get
            {
                var user = GetUser();
                var isAdministrator = user.IsInRole(SystemConstants.AdministratorRoleName);
                return isAdministrator;
            }
        }

        private ClaimsPrincipal GetUser()
        {
            var user = httpContextAccessor.HttpContext.User;

            if (user == null)
            {
                throw new Exception("User is not authenticated.");
            }

            return user;
        }
    }
}
