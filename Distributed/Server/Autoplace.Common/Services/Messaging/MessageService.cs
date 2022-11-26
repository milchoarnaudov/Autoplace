using Autoplace.Common.Data;
using Autoplace.Common.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Autoplace.Common.Services.Messaging
{
    public class MessageService : IMessageService
    {
        private readonly BaseDbContextWithMessaging data;
        private readonly IPublisher publisher;

        public MessageService(DbContext data, IPublisher publisher)
        {
            this.data = data as BaseDbContextWithMessaging
                ?? throw new InvalidOperationException($"Messages can only be used with a {nameof(BaseDbContextWithMessaging)}.");
            this.publisher = publisher;
        }

        public async Task<bool> IsDuplicatedAsync(
            object messageData,
            string propertyFilter,
            object identifier)
        {
            var messageType = messageData.GetType();

            return await data
                .Messages
                .FromSqlRaw($"SELECT * FROM Messages WHERE Type = '{messageType.AssemblyQualifiedName}' AND JSON_VALUE(serializedData, '$.{propertyFilter}') = '{identifier}'")
                .CountAsync() > 0;
        }

        public async Task AddMessageAsync(Message message, bool saveChanges = false)
        {
            await data.Messages.AddAsync(message);

            if (saveChanges)
            {
                await data.SaveChangesAsync();
            }
        }

        public async Task PublishAsync(Message message)
        {
            await publisher.PublishAsync(message.Data);
            await MarkMessageAsPublishedAsync(message.Id);
        }

        private async Task MarkMessageAsPublishedAsync(int id)
        {
            var message = await data.FindAsync<Message>(id);

            message.MarkAsPublished();

            await data.SaveChangesAsync();
        }
    }
}
