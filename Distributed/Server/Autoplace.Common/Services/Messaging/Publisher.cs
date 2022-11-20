using MassTransit;

namespace Autoplace.Common.Services.Messaging
{
    public class Publisher : IPublisher
    {
        private const int TimeoutMilliseconds = 2000;

        private readonly IBus bus;

        public Publisher(IBus bus) => this.bus = bus;

        public Task PublishAsync<TMessage>(TMessage message)
            => bus.Publish(message, GetCancellationToken());

        public Task PublishAsync<TMessage>(TMessage message, Type messageType)
            => bus.Publish(message, messageType, GetCancellationToken());

        private static CancellationToken GetCancellationToken()
        {
            var timeout = TimeSpan.FromMilliseconds(TimeoutMilliseconds);
            var cancellationTokenSource = new CancellationTokenSource(timeout);
            return cancellationTokenSource.Token;
        }
    }
}