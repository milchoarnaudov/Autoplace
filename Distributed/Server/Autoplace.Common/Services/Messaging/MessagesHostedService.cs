using Autoplace.Common.Messaging;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Autoplace.Common.Services.Messaging
{
    public class MessagesHostedService : IHostedService
    {
        private readonly IRecurringJobManager recurringJob;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IPublisher publisher;

        public MessagesHostedService(
            IRecurringJobManager recurringJob,
            IServiceScopeFactory serviceScopeFactory,
            IPublisher publisher)
        {
            this.recurringJob = recurringJob;
            this.serviceScopeFactory = serviceScopeFactory;
            this.publisher = publisher;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceScopeFactory.CreateScope();

            var data = scope.ServiceProvider.GetService<DbContext>();

            if (!data.Database.CanConnect())
            {
                data.Database.Migrate();
            }

            recurringJob.AddOrUpdate(
                nameof(MessagesHostedService),
                () => ProcessPendingMessages(),
                "*/5 * * * * *");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public void ProcessPendingMessages()
        {
            using var scope = serviceScopeFactory.CreateScope();

            var data = scope.ServiceProvider.GetService<DbContext>();

            var messages = data
                .Set<Message>()
                .Where(m => !m.Published)
                .OrderBy(m => m.Id)
                .ToList();

            foreach (var message in messages)
            {
                publisher
                    .PublishAsync(message.Data, message.Type)
                    .GetAwaiter()
                    .GetResult();

                message.MarkAsPublished();

                data.SaveChanges();
            }
        }
    }
}
