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

namespace InvitationCommandService.Application.CommandHandler.Reject
{
    public class RejectInvitationCommandHandle : IRequestHandler<RejectInvitationCommand, int>
    {
        private readonly IEventRepository eventRepository;
        private readonly IServiceBusRepository serviceBus;

        public RejectInvitationCommandHandle(IEventRepository eventRepository,IServiceBusRepository serviceBus)
        {
            this.eventRepository = eventRepository;
            this.serviceBus = serviceBus;
        }
        public async Task<int> Handle(RejectInvitationCommand request, CancellationToken cancellationToken)
        {
            Aggregate aggregate = Aggregate.GenerateAggregate(new RejectInvitationState(), request.subscriptionId, request.memberId);
            EventEntity? eventEntity = await eventRepository.GetLastEventByAggregateId(aggregate.AggregateId);
            aggregate.loadEvent(eventEntity);
            aggregate.CanDoEvent();
            RejectInvitationEventEntity @event = new RejectInvitationEventEntity
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
