using InvitationQueryService.Application.Abstractions;
using MediatR;

namespace InvitationQueryService.Application.QuerySideServiceBus.Reject
{
    public class RejectInvitationQueryHandler : IRequestHandler<RejectInvitationQuery, bool>
    {
        private readonly IInvitationEventsRepository invitationEventsRepository;

        public RejectInvitationQueryHandler(IInvitationEventsRepository invitationEventsRepository)
        {
            this.invitationEventsRepository = invitationEventsRepository;
        }
        public async Task<bool> Handle(RejectInvitationQuery request, CancellationToken cancellationToken)
        {
            int realSequence = await invitationEventsRepository
                .GetSequence(request.Data.MemberId, request.Data.SubscriptionId);
            if (realSequence + 1 < request.Sequence) return false;
            if (realSequence + 1 > request.Sequence) return true;
            if (realSequence + 1 == request.Sequence)
            {
                await invitationEventsRepository.RejectInvitation(request);
                return true;
            }
            return false;
        }
    }
}
