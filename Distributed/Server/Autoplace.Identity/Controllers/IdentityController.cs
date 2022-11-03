using Autoplace.Common.Services;
using Autoplace.Identity.Models.InputModels;
using Autoplace.Identity.Models.OutputModels;
using Autoplace.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autoplace.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService identityService;
        private readonly ICurrentUserService currentUserService;

        public IdentityController(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            this.identityService = identityService;
            this.currentUserService = currentUserService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisteredUserOutputModel>> Register(UserInputModel request)
        {
            var result = await identityService.Register(request);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            var registeredUser = new RegisteredUserOutputModel
            {
                Email = result.Model.Email,
                Username = result.Model.UserName,
            };

            return Ok(registeredUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginInputModel request)
        {
            var result = await identityService.Login(request);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Model);
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordInputModel request)
        {
            var result = await identityService.ResetPassword(request);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok();
        }

        [HttpPost("forgottenPassword")]
        public async Task<ActionResult> ForgottenPassword(ForgottenPasswordInputModel request)
        {
            var result = await identityService.GeneratePasswordResetToken(request);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok();
        }

        [Authorize]
        [HttpPut("changePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordInputModel request)
        {
            var currentUserId = currentUserService.UserId;
            var result = await identityService.ChangePassword(request, currentUserId);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok();
        }
    }
}
