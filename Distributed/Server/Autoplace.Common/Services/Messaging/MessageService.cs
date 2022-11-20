using Autoplace.Common.Data;
using Autoplace.Common.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<bool> IsDuplicated(
            object messageData,
            string propertyFilter,
            object identifier)
        {
            var messageType = messageData.GetType();

            return await data
                .Messages
                .FromSqlRaw($"SELECT * FROM Messages WHERE Type = '{messageType.AssemblyQualifiedName}' AND JSON_VALUE(serializedData, '$.{propertyFilter}') = '{identifier}'")
                .AnyAsync();
        }

        public async Task MarkMessageAsPublished(int id)
        {
            var message = await data.FindAsync<Message>(id);

            message.MarkAsPublished();

            await data.SaveChangesAsync();
        }

        public async Task SaveMessageAsync(Message message)
        {
            data.Messages.Add(message);
            await data.SaveChangesAsync();
        }

        public async Task PublishAsync(object messageData)
        {
            var message = new Message(messageData);

            await SaveMessageAsync(message);

            await publisher.PublishAsync(message.Data);

            await MarkMessageAsPublished(message.Id);
        }
    }
}
