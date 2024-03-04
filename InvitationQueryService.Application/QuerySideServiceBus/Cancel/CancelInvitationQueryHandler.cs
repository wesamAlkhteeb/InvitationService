using InvitationQueryService.Application.Abstractions;
using MediatR;

namespace InvitationQueryService.Application.QuerySideServiceBus.Cancel
{
    public class CancelInvitationQueryHandler : IRequestHandler<CancelInvitationQuery, bool>
    {
        private readonly IInvitationEventsRepository invitationEventsRepository;

        public CancelInvitationQueryHandler(IInvitationEventsRepository invitationEventsRepository)
        {
            this.invitationEventsRepository = invitationEventsRepository;
        }
        public async Task<bool> Handle(CancelInvitationQuery request, CancellationToken cancellationToken)
        {
            int realSequence = await invitationEventsRepository
                .GetSequence(request.Data.MemberId, request.Data.SubscriptionId);
            if (realSequence + 1 < request.Sequence) return false;
            if (realSequence + 1 > request.Sequence) return true;
            if (realSequence + 1 == request.Sequence)
            {
                await invitationEventsRepository.CancelInvitation(request);
                return true;
            }
            return false;
        }
    }
}
