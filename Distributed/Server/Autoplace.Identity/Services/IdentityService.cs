using Autoplace.Common.Models;
using Autoplace.Identity.Data.Models;
using Autoplace.Identity.Models.InputModels;
using Autoplace.Identity.Models.OutputModels;
using Microsoft.AspNetCore.Identity;

namespace Autoplace.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private const string InvalidCredentials = "Invalid credentials";

        private readonly ITokenProviderService tokenProviderService;
        private readonly UserManager<User> userManager;

        public IdentityService(ITokenProviderService tokenProviderService, UserManager<User> userManager)
        {
            this.tokenProviderService = tokenProviderService;
            this.userManager = userManager;
        }

        public async Task<Result<User>> Register(UserInputModel userInputModel)
        {
            var user = new User()
            {
                Email = userInputModel.Email,
                UserName = userInputModel.Username
            };

            var operationResult = await userManager.CreateAsync(user, userInputModel.Password);
            var errors = operationResult.Errors.Select(e => e.Description);

            return operationResult.Succeeded
                ? Result<User>.Success(user)
                : Result<User>.Failure(errors);
        }

        public async Task<Result<UserOutputModel>> Login(LoginInputModel loginInputModel)
        {
            var user = await userManager.FindByEmailAsync(loginInputModel.Email);

            if (user == null)
            {
                return Result<UserOutputModel>.Failure(InvalidCredentials);
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginInputModel.Password);

            if (!isPasswordValid)
            {
                return Result<UserOutputModel>.Failure(InvalidCredentials);
            }

            var roles = await userManager.GetRolesAsync(user);
            var token = tokenProviderService.GenerateToken(user, roles);

            var result = new UserOutputModel()
            {
                Token = token,
            };

            return Result<UserOutputModel>.Success(result);
        }

        public async Task<Result> ChangePassword(ChangePasswordInputModel changePasswordInputModel, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Result.Failure(InvalidCredentials);
            }

            var operationResult = await userManager.ChangePasswordAsync(
                user,
                changePasswordInputModel.CurrentPassword,
                changePasswordInputModel.NewPassword);

            var errors = operationResult.Errors.Select(e => e.Description);

            return operationResult.Succeeded
                ? Result.Success()
                : Result.Failure(errors);
        }

        public async Task<Result> ResetPassword(ResetPasswordInputModel resetPasswordInputModel)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordInputModel.Email);

            if (user == null)
            {
                return Result.Failure(InvalidCredentials);
            }

            var operationResult = await userManager.ResetPasswordAsync(user, resetPasswordInputModel.Token, resetPasswordInputModel.Password);
            var errors = operationResult.Errors.Select(e => e.Description);

            return operationResult.Succeeded
                ? Result.Success()
                : Result.Failure(errors);
        }

        public async Task<Result> GeneratePasswordResetToken(ForgottenPasswordInputModel forgottenPasswordInputModel)
        {
            var user = await userManager.FindByEmailAsync(forgottenPasswordInputModel.Email);

            if (user == null)
            {
                return Result.Failure(InvalidCredentials);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            
            // Send token by email

            return Result.Success();
        }
    }
}
