using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InvitationQueryService.Application.QuerySideServiceBus.Leave
{
    public class LeaveInvitationQueryHandler : IRequestHandler<LeaveInvitationQuery, bool>
    {
        private readonly IInvitationEventsRepository invitationEventsRepository;
        private readonly ILogger<LeaveInvitationQueryHandler> logger;

        public LeaveInvitationQueryHandler(
            IInvitationEventsRepository invitationEventsRepository,
            ILogger<LeaveInvitationQueryHandler> logger)
        {
            this.invitationEventsRepository = invitationEventsRepository;
            this.logger = logger;
        }
        public async Task<bool> Handle(LeaveInvitationQuery request, CancellationToken cancellationToken)
        {
            SubscriptorEntity? subscriptor = await invitationEventsRepository
                .GetSubscriptor(request.Data.MemberId, request.Data.SubscriptionId);

            if (subscriptor == null)
            {
                logger.LogWarning("I don't have send event record in database and receive accept Event");
                return false;
            }
            else if (subscriptor.Sequence + 1 == request.Sequence)
            {
                subscriptor.Status = InvitationState.Out.ToString();
                subscriptor.Sequence = request.Sequence;
                await invitationEventsRepository.Complete();
                return true;
            }
            else if (subscriptor.Sequence + 1 < request.Sequence) return false;
            else if (subscriptor.Sequence + 1 > request.Sequence) return true;
            logger.LogWarning("there are handle error");

            return false;
        }
    }
}
