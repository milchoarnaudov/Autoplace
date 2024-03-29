﻿using AutoMapper;
using Autoplace.Common.Errors;
using Autoplace.Common.Messaging;
using Autoplace.Common.Messaging.Users;
using Autoplace.Common.Services;
using Autoplace.Common.Services.Data;
using Autoplace.Common.Services.Messaging;
using Autoplace.Identity.Common;
using Autoplace.Identity.Data.Models;
using Autoplace.Identity.Models.InputModels;
using Autoplace.Identity.Models.OutputModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Autoplace.Identity.Services
{
    public class IdentityService : BaseDataService<User>, IIdentityService
    {
        private readonly ITokenProviderService tokenProviderService;
        private readonly UserManager<User> userManager;
        private readonly IPublisher publisher;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public IdentityService(
            DbContext dbContext,
            ITokenProviderService tokenProviderService,
            UserManager<User> userManager,
            IPublisher publisher,
            ILogger logger,
            IMapper mapper)
            : base(dbContext)
        {
            this.tokenProviderService = tokenProviderService;
            this.userManager = userManager;
            this.publisher = publisher;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<OperationResult<RegisteredUserOutputModel>> Register(UserInputModel userInputModel)
        {
            var user = new User()
            {
                Email = userInputModel.Email,
                UserName = userInputModel.Username
            };

            var operationResult = await userManager.CreateAsync(user, userInputModel.Password);
            var errors = operationResult.Errors.Select(e => e.Description);

            if (!operationResult.Succeeded)
            {
                return OperationResult<RegisteredUserOutputModel>.Failure(errors);
            }

            try
            {
                await SendMessageAsync(user.Id, user.Email, user.UserName);
            }
            catch (Exception e)
            {
                logger.LogError(e, GenericErrorMessages.ErrorWhilePublishingMessageErrorMessage);
            }

            var outputModel = mapper.Map<RegisteredUserOutputModel>(user);

            return OperationResult<RegisteredUserOutputModel>.Success(outputModel);
        }


        public async Task<OperationResult<LoggedInUserOutputModel>> Login(LoginInputModel loginInputModel)
        {
            var user = await userManager.FindByEmailAsync(loginInputModel.Email);

            if (user == null)
            {
                return OperationResult<LoggedInUserOutputModel>.Failure(ErrorMessages.InvalidCredentials);
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginInputModel.Password);

            if (!isPasswordValid)
            {
                return OperationResult<LoggedInUserOutputModel>.Failure(ErrorMessages.InvalidCredentials);
            }

            var roles = await userManager.GetRolesAsync(user);
            var token = tokenProviderService.GenerateToken(user, roles);

            var result = new LoggedInUserOutputModel()
            {
                Token = token,
            };

            return OperationResult<LoggedInUserOutputModel>.Success(result);
        }

        public async Task<OperationResult> ChangePassword(ChangePasswordInputModel changePasswordInputModel, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return OperationResult.Failure(ErrorMessages.InvalidCredentials);
            }

            var operationResult = await userManager.ChangePasswordAsync(
                user,
                changePasswordInputModel.CurrentPassword,
                changePasswordInputModel.NewPassword);

            var errors = operationResult.Errors.Select(e => e.Description);

            return operationResult.Succeeded
                ? OperationResult.Success()
                : OperationResult.Failure(errors);
        }

        public async Task<OperationResult> ResetPassword(ResetPasswordInputModel resetPasswordInputModel)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordInputModel.Email);

            if (user == null)
            {
                return OperationResult.Failure(ErrorMessages.InvalidCredentials);
            }

            var operationResult = await userManager.ResetPasswordAsync(user, resetPasswordInputModel.Token, resetPasswordInputModel.Password);
            var errors = operationResult.Errors.Select(e => e.Description);

            return operationResult.Succeeded
                ? OperationResult.Success()
                : OperationResult.Failure(errors);
        }

        public async Task<OperationResult> GeneratePasswordResetToken(ForgottenPasswordInputModel forgottenPasswordInputModel)
        {
            var user = await userManager.FindByEmailAsync(forgottenPasswordInputModel.Email);

            if (user == null)
            {
                return OperationResult.Failure(ErrorMessages.InvalidCredentials);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            // Send token by email

            return OperationResult.Success();
        }

        private async Task SendMessageAsync(string userId, string email, string username)
        {
            var messageData = new UserRegisteredMessage
            {
                UserId = userId,
                Email = email,
                Username = username,
            };

            var message = new Message(messageData);

            Data.Add(message);

            await Data.SaveChangesAsync();

            await publisher.PublishAsync(messageData);

            message.MarkAsPublished();

            await Data.SaveChangesAsync();
        }
    }
}
