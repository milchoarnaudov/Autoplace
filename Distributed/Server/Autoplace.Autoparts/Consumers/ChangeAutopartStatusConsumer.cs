using Autoplace.Autoparts.Services;
using Autoplace.Common.Messaging;
using Autoplace.Common.Messaging.Autoparts;
using Autoplace.Common.Services.Messaging;
using MassTransit;

namespace Autoplace.Autoparts.Consumers
{
    public class ChangeAutopartStatusConsumer : IConsumer<ChangeAutopartStatusMessage>
    {
        private readonly IMessageService messageService;
        private readonly IAutopartsService autopartsService;

        public ChangeAutopartStatusConsumer(IMessageService messageService, IAutopartsService autopartsService)
        {
            this.messageService = messageService;
            this.autopartsService = autopartsService;
        }

        public async Task Consume(ConsumeContext<ChangeAutopartStatusMessage> context)
        {
            var message = context.Message;
            var isDuplicated = await messageService.IsDuplicatedAsync(
               message,
               nameof(ChangeAutopartStatusMessage.MessageDataId),
               message.MessageDataId);

            if (isDuplicated)
            {
                return;
            }

            var approvalRequest = context.Message;
            var result = await autopartsService.ChangeStatus(approvalRequest.AutopartId, approvalRequest.NewStatus);

            if (!result.IsSuccessful)
            {
                throw new Exception(string.Join(Environment.NewLine, result.ErrorMessages));
            }

            await messageService.AddMessageAsync(new Message(message, true), true);
        }
    }
}
