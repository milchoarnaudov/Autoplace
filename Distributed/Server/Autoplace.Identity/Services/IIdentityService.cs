using Autoplace.Common.Models;
using Autoplace.Identity.Data.Models;
using Autoplace.Identity.Models.InputModels;
using Autoplace.Identity.Models.OutputModels;

namespace Autoplace.Identity.Services
{
    public interface IIdentityService
    {
        Task<Result<User>> Register(UserInputModel userInputModel);

        Task<Result<UserOutputModel>> Login(LoginInputModel loginInputModel);

        Task<Result> ChangePassword(ChangePasswordInputModel changePasswordInputModel, string userId);

        Task<Result> ResetPassword(ResetPasswordInputModel resetPasswordInputModel);

        Task<Result> GeneratePasswordResetToken(ForgottenPasswordInputModel forgottenPasswordInputModel);
    }
}
