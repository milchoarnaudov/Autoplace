using Autoplace.Common.Controllers;
using Autoplace.Common.Services.Identity;
using Autoplace.Identity.Models.InputModels;
using Autoplace.Identity.Models.OutputModels;
using Autoplace.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autoplace.Identity.Controllers
{
    public class IdentityController : BaseApiController
    {
        private readonly IIdentityService identityService;
        private readonly ICurrentUserService currentUserService;

        public IdentityController(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            this.identityService = identityService;
            this.currentUserService = currentUserService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisteredUserOutputModel>> Register(UserInputModel input)
        {
            var result = await identityService.Register(input);

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
        public async Task<ActionResult> Login(LoginInputModel input)
        {
            var result = await identityService.Login(input);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Model);
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordInputModel input)
        {
            var result = await identityService.ResetPassword(input);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok();
        }

        [HttpPost("forgottenPassword")]
        public async Task<ActionResult> ForgottenPassword(ForgottenPasswordInputModel input)
        {
            var result = await identityService.GeneratePasswordResetToken(input);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok();
        }

        [Authorize]
        [HttpPut("changePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordInputModel input)
        {
            var currentUserId = currentUserService.UserId;
            var result = await identityService.ChangePassword(input, currentUserId);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok();
        }
    }
}
