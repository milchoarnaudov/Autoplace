namespace Autoplace.Common.Services.Messaging
{
    using Autoplace.Common.Messaging;
    using System.Threading.Tasks;

    public interface IMessageService
    {
        Task<bool> IsDuplicated(
            object messageData,
            string propertyFilter,
            object identifier);

        Task PublishAsync(object messageData);

        Task SaveMessageAsync(Message message);

        Task MarkMessageAsPublished(int id);

    }
}
