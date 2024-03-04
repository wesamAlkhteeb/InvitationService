using InvitationQueryService.Application.Abstractions;
using MediatR;

namespace InvitationQueryService.Application.QuerySideServiceBus.Send
{
    public class SendInvitationQueryHandler : IRequestHandler<SendInvitationQuery, bool>
    {
        private readonly IInvitationEventsRepository invitationEventsRepository;

        public SendInvitationQueryHandler(IInvitationEventsRepository invitationEventsRepository)
        {
            this.invitationEventsRepository = invitationEventsRepository;
        }
        public async Task<bool> Handle(SendInvitationQuery request, CancellationToken cancellationToken)
        {
            int realSequence = await invitationEventsRepository
                .GetSequence(request.Data.Info.MemberId, request.Data.Info.SubscriptionId);
            if (realSequence + 1 < request.Sequence) return false;
            if (realSequence + 1 > request.Sequence) return true;
            if (realSequence + 1 == request.Sequence)
            {
                await invitationEventsRepository.SendInvitation(request);
                return true;
            }
            return false;
        }
    }
}
