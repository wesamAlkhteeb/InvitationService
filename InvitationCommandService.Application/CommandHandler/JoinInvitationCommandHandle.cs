﻿using InvitationCommandService.Application.Abstraction;
using InvitationCommandService.Domain.Domain;
using InvitationCommandService.Domain.Entities.Data;
using InvitationCommandService.Domain.Entities.Events;
using InvitationCommandService.Domain.Model.Command;
using InvitationCommandService.Domain.StateInvitation;
using MediatR;

namespace InvitationCommandService.Application.CommandHandler
{
    internal class JoinInvitationCommandHandle : IRequestHandler<JoinInvitationCommand, int>
    {
        private readonly IEventRepository eventRepository;
        private readonly IServiceBusRepository serviceBus;

        public JoinInvitationCommandHandle(IEventRepository eventRepository, IServiceBusRepository serviceBus)
        {
            this.eventRepository = eventRepository;
            this.serviceBus = serviceBus;
        }
        public async Task<int> Handle(JoinInvitationCommand request, CancellationToken cancellationToken)
        {
            Aggregate aggregate = GeneralAggregate.GenerateAggregate(new JoinInvitationState(), request.subscriptionId, request.memberId);
            EventEntity? eventEntity = await eventRepository.GetLastEventByAggregateId(aggregate.AggregateId);
            aggregate.loadEvent(eventEntity);
            aggregate.CanDoEvent();
            JoinInvitationEventEntity @event = new JoinInvitationEventEntity
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
