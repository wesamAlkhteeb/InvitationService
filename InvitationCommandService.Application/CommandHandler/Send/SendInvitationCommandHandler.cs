using InvitationCommandService.Application.Abstraction;
using InvitationCommandService.Domain.Domain;
using InvitationCommandService.Domain.Entities.Data;
using InvitationCommandService.Domain.Entities.Events;
using InvitationCommandService.Domain.StateInvitation;
using MediatR;

namespace InvitationCommandService.Application.CommandHandler.Send
{
    public class SendInvitationCommandHandler(IEventRepository eventRepository, IServiceBusRepository serviceBus) : IRequestHandler<SendInvitationCommand, int>
    {
        private readonly IEventRepository eventRepository = eventRepository;

        public IServiceBusRepository serviceBus { get; } = serviceBus;

        public async Task<int> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
        {
            Aggregate aggregate = Aggregate.GenerateAggregate(new SendInvitationState(), request.subscriptionId, request.memberId);
            EventEntity? eventEntity = await eventRepository.GetLastEventByAggregateId(aggregate.AggregateId);
            aggregate.loadEvent(eventEntity);
            aggregate.CanDoEvent();
            
            SendInvitationEventEntity @event = new SendInvitationEventEntity
            {
                AggregateId = aggregate.AggregateId,
                DateTime = DateTime.UtcNow,
                Sequence = aggregate.NextSequence,
                Data = new InvitationData
                {
                    Info = new InvitationInfoData
                    {
                        AccountId = request.accountId,
                        MemberId = request.memberId,
                        SubscriptionId = request.subscriptionId,
                        UserId = request.userId,
                    },
                    Permissions = request.Permissions
                }
            };
            await eventRepository.AddEvent(@event);

            await serviceBus.PublicMessage();
            return @event.Id;
        }
    }
}
