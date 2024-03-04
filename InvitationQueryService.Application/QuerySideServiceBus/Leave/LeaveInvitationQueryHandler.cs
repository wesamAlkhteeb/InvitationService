using InvitationQueryService.Application.Abstractions;
using MediatR;

namespace InvitationQueryService.Application.QuerySideServiceBus.Leave
{
    public class LeaveInvitationQueryHandler : IRequestHandler<LeaveInvitationQuery, bool>
    {
        private readonly IInvitationEventsRepository invitationEventsRepository;

        public LeaveInvitationQueryHandler(IInvitationEventsRepository invitationEventsRepository)
        {
            this.invitationEventsRepository = invitationEventsRepository;
        }
        public async Task<bool> Handle(LeaveInvitationQuery request, CancellationToken cancellationToken)
        {
            int realSequence = await invitationEventsRepository
                .GetSequence(request.Data.MemberId, request.Data.SubscriptionId);
            if (realSequence + 1 < request.Sequence) return false;
            if (realSequence + 1 > request.Sequence) return true;
            if (realSequence + 1 == request.Sequence)
            {
                await invitationEventsRepository.LeaveInvitation(request);
                return true;
            }
            return false;
        }
    }
}
