using InvitationQueryService.Application.Abstraction;
using InvitationQueryService.Domain.Domain;
using InvitationQueryService.Domain.Entities.Data;
using InvitationQueryService.Domain.Entities.Events;
using InvitationQueryService.Domain.StateInvitation;
using MediatR;

namespace InvitationQueryService.Application.CommandHandler.ChangePermissions
{
    public class ChangePermissionsInvitationCommandHandle : IRequestHandler<ChangePermissionInvitationCommand, int>
    {
        private readonly IEventRepository eventRepository;
        private readonly IServiceBusRepository serviceBus;

        public ChangePermissionsInvitationCommandHandle(IEventRepository eventRepository, IServiceBusRepository serviceBus)
        {
            this.eventRepository = eventRepository;
            this.serviceBus = serviceBus;
        }
        public async Task<int> Handle(ChangePermissionInvitationCommand request, CancellationToken cancellationToken)
        {
            Aggregate aggregate = Aggregate.GenerateAggregate(new ChangePermissionsInvitationState(), request.subscriptionId, request.memberId);
            EventEntity? eventEntity = await eventRepository.GetLastEventByAggregateId(aggregate.AggregateId);
            aggregate.loadEvent(eventEntity);
            aggregate.CanDoEvent();
            ChangePermissionsInvitationEventEntity @event = new ChangePermissionsInvitationEventEntity
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
