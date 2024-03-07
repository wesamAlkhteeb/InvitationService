using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InvitationQueryService.Application.QuerySideServiceBus.Accept
{
    public class AcceptInvitationQueryHandler : IRequestHandler<AcceptInvitationQuery, bool>
    {
        private readonly IInvitationEventsRepository invitationEventsRepository;
        private readonly ILogger<AcceptInvitationQueryHandler> logger;

        public AcceptInvitationQueryHandler(IInvitationEventsRepository invitationEventsRepository, ILogger<AcceptInvitationQueryHandler> logger)
        {
            this.invitationEventsRepository = invitationEventsRepository;
            this.logger = logger;
        }
        public async Task<bool> Handle(AcceptInvitationQuery request, CancellationToken cancellationToken)
        {
            SubscriptorEntity? subscriptor = await invitationEventsRepository
                .GetSubscriptor(request.Data.MemberId, request.Data.SubscriptionId);

            if (subscriptor == null)
            {
                logger.LogWarning("I don't have send event record in database and receive accept Event");
                return false;
            }
            else if (subscriptor.Sequence == request.Sequence)
            {
                subscriptor.Status = InvitationState.Joined.ToString();
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
