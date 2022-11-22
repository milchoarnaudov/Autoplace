using Autoplace.Administration.Data.Models;
using Autoplace.Administration.Services;
using Autoplace.Common.Messaging;
using Autoplace.Common.Messaging.Autoparts;
using Autoplace.Common.Services.Messaging;
using MassTransit;

namespace Autoplace.Administration.Consumers
{
    public class ApprovalRequestConsumer : IConsumer<ApprovalRequestMessage>
    {
        private readonly IMessageService messageService;
        private readonly IApprovalRequestsService approvalRequestsService;

        public ApprovalRequestConsumer(IMessageService messageService, IApprovalRequestsService approvalRequestsService)
        {
            this.messageService = messageService;
            this.approvalRequestsService = approvalRequestsService;
        }

        public async Task Consume(ConsumeContext<ApprovalRequestMessage> context)
        {
            var message = context.Message;
            var isDuplicated = await messageService.IsDuplicated(
               message,
               nameof(ApprovalRequestMessage.MessageId),
               message.MessageId);

            if (isDuplicated)
            {
                return;
            }

            var requestApproval = context.Message;
            var result = await approvalRequestsService.CreateAsync(
                requestApproval.AutopartId,
                requestApproval.Name,
                requestApproval.Description,
                requestApproval.Price,
                requestApproval.Username,
                requestApproval.Images.Select(i =>
                new Image
                {
                    FileId = i.Id,
                    Extension = i.Extension,
                    RemoteImageUrl = i.RemoteImageUrl,
                }));

            if (!result.IsSuccessful)
            {
                throw new Exception(string.Join(Environment.NewLine, result.ErrorMessages));
            }

            await messageService.SaveMessageAsync(new Message(message, true));
         }
    }
}
