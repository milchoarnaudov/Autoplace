namespace Autoplace.Common.Services.Messaging
{
    using Autoplace.Common.Messaging;
    using System.Threading.Tasks;

    public interface IMessageService
    {
        Task<bool> IsDuplicatedAsync(
            object messageData,
            string propertyFilter,
            object identifier);

        Task PublishAsync(Message message);

        Task AddMessageAsync(Message message, bool saveChanges = false);
    }
}
