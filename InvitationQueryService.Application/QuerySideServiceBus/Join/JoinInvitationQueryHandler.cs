using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Domain;
using MediatR;
using Microsoft.Extensions.Logging;


namespace InvitationQueryService.Application.QuerySideServiceBus.Join
{
    public class JoinInvitationQueryHandler : IRequestHandler<JoinInvitationQuery, bool>
    {
        private readonly IInvitationEventsRepository invitationEventsRepository;
        private readonly ILogger<JoinInvitationQueryHandler> logger;

        public JoinInvitationQueryHandler(IInvitationEventsRepository invitationEventsRepository,ILogger<JoinInvitationQueryHandler>logger)
        {
            this.invitationEventsRepository = invitationEventsRepository;
            this.logger = logger;
        }
        public async Task<bool> Handle(JoinInvitationQuery request, CancellationToken cancellationToken)
        {
            SubscriptorEntity? subscriptor = await invitationEventsRepository
                .GetSubscriptor(request.Data.Info.MemberId, request.Data.Info.SubscriptionId);

            if (subscriptor == null)
            {
                await invitationEventsRepository.JoinInvitation(request);
                return true;
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
