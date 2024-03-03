using InvitationQueryService.Application.Abstractions;
using MediatR;


namespace InvitationQueryService.Application.QuerySide.Join
{
    public class JoinInvitationQueryHandler : IRequestHandler<JoinInvitationQuery, bool>
    {
        private readonly IInvitationEventsRepository invitationEventsRepository;

        public JoinInvitationQueryHandler(IInvitationEventsRepository invitationEventsRepository)
        {
            this.invitationEventsRepository = invitationEventsRepository;
        }
        public async Task<bool> Handle(JoinInvitationQuery request, CancellationToken cancellationToken)
        {
            int realSequence = await invitationEventsRepository
                .GetSequence(request.Data.Info.MemberId, request.Data.Info.SubscriptionId);
            if (realSequence + 1 < request.Sequence) return false;
            if (realSequence + 1 > request.Sequence) return true;
            if (realSequence + 1 == request.Sequence)
            {
                await invitationEventsRepository.JoinInvitation(request);
                return true;
            }
            return false;
        }
    }
}
