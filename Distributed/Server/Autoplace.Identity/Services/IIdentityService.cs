using Autoplace.Common.Models;
using Autoplace.Identity.Models.InputModels;
using Autoplace.Identity.Models.OutputModels;

namespace Autoplace.Identity.Services
{
    public interface IIdentityService
    {
        Task<OperationResult<RegisteredUserOutputModel>> Register(UserInputModel userInputModel);

        Task<OperationResult<LoggedInUserOutputModel>> Login(LoginInputModel loginInputModel);

        Task<OperationResult> ChangePassword(ChangePasswordInputModel changePasswordInputModel, string userId);

        Task<OperationResult> ResetPassword(ResetPasswordInputModel resetPasswordInputModel);

        Task<OperationResult> GeneratePasswordResetToken(ForgottenPasswordInputModel forgottenPasswordInputModel);
    }
}
