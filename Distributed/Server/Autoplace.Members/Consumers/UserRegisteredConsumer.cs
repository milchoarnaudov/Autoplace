﻿using AutoMapper;
using Autoplace.Common.Messaging;
using Autoplace.Common.Messaging.Users;
using Autoplace.Common.Services.Messaging;
using Autoplace.Members.Services;
using MassTransit;

namespace Autoplace.Members.Consumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredMessage>
    {
        private readonly IMembersService membersService;
        private readonly IMessageService messageService;

        public UserRegisteredConsumer(IMembersService membersService, IMessageService messageService)
        {
            this.membersService = membersService;
            this.messageService = messageService;
        }

        public async Task Consume(ConsumeContext<UserRegisteredMessage> context)
        {
            var message = context.Message;
            var isDuplicated = await messageService.IsDuplicated(
               message,
               nameof(UserRegisteredMessage.UserId),
               message.UserId);

            if (isDuplicated)
            {
                return;
            }

            var result = await membersService.CreateAsync(context.Message.UserId, context.Message.Username, context.Message.Email);

            if (!result.IsSuccessful)
            {
                throw new Exception(string.Join(Environment.NewLine, result.ErrorMessages));
            }

            await messageService.SaveMessageAsync(new Message(message, true));
        }
    }
}