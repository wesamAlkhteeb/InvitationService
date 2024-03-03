using InvitationCommandService.Application.Abstraction;
using InvitationCommandService.Domain;
using InvitationCommandService.Domain.Domain;
using InvitationCommandService.Domain.Entities.Data;
using InvitationCommandService.Domain.Entities.Events;
using InvitationCommandService.Domain.StateInvitation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationCommandService.Application.CommandHandler.Cancel
{
    public class CancelInvitationCommandHandle : IRequestHandler<CancelInvitationCommand, int>
    {
        private readonly IEventRepository eventRepository;
        private readonly IServiceBusRepository serviceBus;

        public CancelInvitationCommandHandle(IEventRepository eventRepository ,IServiceBusRepository serviceBus)
        {
            this.eventRepository = eventRepository;
            this.serviceBus = serviceBus;
        }
        public async Task<int> Handle(CancelInvitationCommand request, CancellationToken cancellationToken)
        {
            Aggregate aggregate = Aggregate.GenerateAggregate(new CancelInvitationState(), request.subscriptionId, request.memberId);
            EventEntity? eventEntity = await eventRepository.GetLastEventByAggregateId(aggregate.AggregateId);
            aggregate.loadEvent(eventEntity);
            aggregate.CanDoEvent();
            CancelInvitationEventEntity @event = new CancelInvitationEventEntity
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
