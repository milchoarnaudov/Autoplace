using Autoplace.Common.Models;
using Autoplace.Identity.Data.Models;
using Autoplace.Identity.Models.InputModels;
using Autoplace.Identity.Models.OutputModels;

namespace Autoplace.Identity.Services
{
    public interface IIdentityService
    {
        Task<OperationResult<User>> Register(UserInputModel userInputModel);

        Task<OperationResult<UserOutputModel>> Login(LoginInputModel loginInputModel);

        Task<OperationResult> ChangePassword(ChangePasswordInputModel changePasswordInputModel, string userId);

        Task<OperationResult> ResetPassword(ResetPasswordInputModel resetPasswordInputModel);

        Task<OperationResult> GeneratePasswordResetToken(ForgottenPasswordInputModel forgottenPasswordInputModel);
    }
}
