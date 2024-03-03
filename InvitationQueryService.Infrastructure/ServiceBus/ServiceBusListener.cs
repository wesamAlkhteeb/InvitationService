using Azure.Messaging.ServiceBus;
using InvitationQueryService.Infrastructure.ServiceBus;
using Microsoft.Extensions.Hosting;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MediatR;
using Newtonsoft.Json;
using InvitationQueryService.Application.QuerySide.Send;
using InvitationQueryService.Application.QuerySide.ChangePermission;
using InvitationQueryService.Application.QuerySide.Join;
using InvitationQueryService.Application.QuerySide.Cancel;
using InvitationQueryService.Application.QuerySide.Reject;
using InvitationQueryService.Application.QuerySide.Remove;
using InvitationQueryService.Application.QuerySide.Leave;
using InvitationQueryService.Application.QuerySide.Accept;

namespace InvitationCommandService.Application.ServiceBus
{
    public class InvitationListener : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<InvitationListener> logger;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSessionProcessor _processor;
        private ServiceBusProcessor _deadLetterProcessor;

        public InvitationListener(AzureOptions azure, IServiceProvider service, ILogger<InvitationListener> logger)
        {
            this._serviceProvider = service;
            this.logger = logger;
            this._client = new ServiceBusClient(azure.ConnectionString);
            _processor = this._client.CreateSessionProcessor(
                topicName: azure.TopicName,
                subscriptionName: azure.SubscriptionName,
                options: new ServiceBusSessionProcessorOptions
                {
                    PrefetchCount = 1,
                    MaxConcurrentSessions = 100,
                    MaxConcurrentCallsPerSession = 1
                });
            _processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
            _processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
            
            _deadLetterProcessor = _client.CreateProcessor(
                topicName: azure.TopicName,
                subscriptionName: azure.SubscriptionName,
                options: new ServiceBusProcessorOptions()
                {
                    AutoCompleteMessages = false,
                    PrefetchCount = 10,
                    MaxConcurrentCalls = 10,
                    SubQueue = SubQueue.DeadLetter,
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                });
            _deadLetterProcessor.ProcessMessageAsync += DeadLetterProcessor_ProcessMessageAsync;
            _deadLetterProcessor.ProcessErrorAsync += Processor_ProcessErrorAsync;
        }

        private async Task DeadLetterProcessor_ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            string json = Encoding.UTF8.GetString(args.Message.Body);
            bool isHandled = await HandleMessage(json, args.Message.Subject);
            if (isHandled)
            {
                await args.CompleteMessageAsync(args.Message);
            }
            else
            {
                logger.LogWarning("Message {MessageId} not handled", args.Message.MessageId);
                await Task.Delay(5000);
                await args.AbandonMessageAsync(args.Message);
            }
        }

        private async Task Processor_ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            logger.LogError("Message {MessageId} not handled", args.ErrorSource);
        }

        private async Task Processor_ProcessMessageAsync(ProcessSessionMessageEventArgs args)
        {
            var json = Encoding.UTF8.GetString(args.Message.Body);
            bool isHandled = await HandleMessage(json,args.Message.Subject);

            if (isHandled)
            {
                await args.CompleteMessageAsync(args.Message);
            }
            else
            {
                logger.LogWarning("Message {MessageId} not handled", args.Message.MessageId);
                await Task.Delay(5000);
                await args.AbandonMessageAsync(args.Message);
            }
        }

        private async Task<bool> HandleMessage(string message, string subject)
        {
            
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            
            switch (subject)
            {
                case "SendEvent":
                    {
                        SendInvitationQuery sendInvitation = JsonConvert.DeserializeObject<SendInvitationQuery>(message)!;
                        return await mediator.Send(sendInvitation);
                    }
                case "ChangepermissionEvent":
                    {
                        ChangePermissionsInvitationQuery permissionsInvitationQuery = JsonConvert.DeserializeObject<ChangePermissionsInvitationQuery>(message)!;
                        return await mediator.Send(permissionsInvitationQuery);
                    }
                case "JoinEvent":
                    {
                        JoinInvitationQuery joinInvitation = JsonConvert.DeserializeObject<JoinInvitationQuery>(message)!;
                        return await mediator.Send(joinInvitation);
                    }
                case "CancelEvent":
                    {
                        CancelInvitationQuery cancelInvitation = JsonConvert.DeserializeObject<CancelInvitationQuery>(message)!;
                        return await mediator.Send(cancelInvitation);
                    }
                case "RejectEvent":
                    {
                        RejectInvitationQuery rejectInvitation = JsonConvert.DeserializeObject<RejectInvitationQuery>(message)!;
                        return await mediator.Send(rejectInvitation);
                    }
                case "RemoveEvent":
                    {
                        RemoveInvitationQuery removeInvitation = JsonConvert.DeserializeObject<RemoveInvitationQuery>(message)!;
                        return await mediator.Send(removeInvitation);
                    }
                case "LeaveEvent":
                    {
                        LeaveInvitationQuery leaveInvitation = JsonConvert.DeserializeObject<LeaveInvitationQuery>(message)!;
                        return await mediator.Send(leaveInvitation);
                    }
                case "AcceptEvent":
                    {
                        AcceptInvitationQuery acceptInvitation = JsonConvert.DeserializeObject<AcceptInvitationQuery>(message)!;
                        return await mediator.Send(acceptInvitation);
                    }
                default:
                    {
                        throw new InvalidOperationException("");
                    }

            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _processor.StartProcessingAsync(cancellationToken);
        }
        public async Task StopAsync(CancellationToken cancellationToken) => await _processor.StartProcessingAsync(cancellationToken);
    }


}
