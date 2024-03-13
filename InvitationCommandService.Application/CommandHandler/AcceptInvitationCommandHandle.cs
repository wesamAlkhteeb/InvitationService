using InvitationCommandService.Application.Abstraction;
using InvitationCommandService.Domain.Domain;
using InvitationCommandService.Domain.Entities.Data;
using InvitationCommandService.Domain.Entities.Events;
using InvitationCommandService.Domain.Model.Command;
using InvitationCommandService.Domain.StateInvitation;
using MediatR;

namespace InvitationCommandService.Application.CommandHandler
{
    public class AcceptInvitationCommandHandle : IRequestHandler<AcceptInvitationCommand, int>
    {
        private readonly IEventRepository eventRepository;
        private readonly IServiceBusRepository serviceBus;

        public AcceptInvitationCommandHandle(IEventRepository eventRepository, IServiceBusRepository serviceBus)
        {
            this.eventRepository = eventRepository;
            this.serviceBus = serviceBus;
        }
        public async Task<int> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
        {
            Aggregate aggregate = GeneralAggregate.GenerateAggregate(new AcceptInvitationState(), request.subscriptionId, request.memberId);
            EventEntity? eventEntity = await eventRepository.GetLastEventByAggregateId(aggregate.AggregateId);
            aggregate.loadEvent(eventEntity);
            aggregate.CanDoEvent();
            AcceptInvitationEventEntity @event = new AcceptInvitationEventEntity
            {
                AggregateId = aggregate.AggregateId,
                DateTime = DateTime.UtcNow,
                Sequence = aggregate.NextSequence,
                Data = new InvitationInfoData
                {
                    AccountId = request.accountId,
                    MemberId = request.memberId,
                    SubscriptionId = request.subscriptionId,
                    UserId = request.userId,
                }
            };
            await eventRepository.AddEvent(@event);
            await serviceBus.PublicMessage();
            return @event.Id;
        }
    }
}
