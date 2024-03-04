using Azure.Messaging.ServiceBus;
using InvitationCommandService.Database;
using InvitationCommandService.Infrastructure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace InvitationCommandService.Application.ServiceBus
{
    public class ServiceBusPublisher
    {
        protected readonly ServiceBusSender _sender;
        protected readonly IServiceProvider _provider;
        private readonly object _lockObject = new();
        private bool IsBusy { get; set; }

        public ServiceBusPublisher(AzureOptions azure, IServiceProvider provider)
        {
            ServiceBusClient client = new ServiceBusClient(azure.ConnectionString);
            _sender = client.CreateSender(azure.TopicName);
            _provider = provider;
        }

        public async Task StartPublishing()
        {

            await PublishEvents();
            //Task.Run(() =>
            //{
            //    if (IsBusy) return;
            //    IsBusy = true;
            //    lock (_lockObject)
            //    {
            //        PublishEvents(subscription.ToString()).GetAwaiter().GetResult();
            //    }
            //    IsBusy = false;
            //});

        }

        private async Task PublishEvents()
        {
            using var scope = _provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();

            while (true)
            {

                var messages = await dbContext.Outboxes
                    .Include(x => x.Event)
                    .OrderBy(x => x.Id)
                    .Take(100)
                    .ToListAsync();

                if (messages.Count == 0) return;

                foreach (var message in messages)
                {
                    if (message.Event is null)
                    {
                        throw new InvalidOperationException("Event is null, please include the event in the query");
                    }
                    var data = new
                    {
                        id = message.Event.Id,
                        aggregateId = message.Event.AggregateId,
                        sequence = message.Event.Sequence,
                        dateTime = message.Event.DateTime,
                        data = message.Event.GetData()
                    };
                    var json = JsonSerializer.Serialize(data);

                    var serviceBusMessage = new ServiceBusMessage(json)
                    {
                        PartitionKey = message.Event.AggregateId.ToString(),
                        SessionId = message.Event.AggregateId.ToString(),
                        Subject = message.Event.Type
                    };

                    await _sender.SendMessageAsync(serviceBusMessage);

                    dbContext.Outboxes.Remove(message);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }


}
