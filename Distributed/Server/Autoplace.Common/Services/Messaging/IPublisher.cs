namespace Autoplace.Common.Services.Messaging
{
    public interface IPublisher
    {
        Task PublishAsync<TMessage>(TMessage message);

        Task PublishAsync<TMessage>(TMessage message, Type messageType);
    }
}
