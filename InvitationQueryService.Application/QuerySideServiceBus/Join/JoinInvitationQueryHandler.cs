using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using InvitationQueryService.Models.QuerySideServiceBus;


namespace InvitationQueryService.Application.QuerySideServiceBus.Join
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
            SubscriptorEntity? subscriptor = await invitationEventsRepository
                .GetSubscriptor(request.Data.Info.MemberId, request.Data.Info.SubscriptionId);

            if (subscriptor == null)
            {
                await invitationEventsRepository.JoinInvitation(request);
                return true;
            }
            else if (subscriptor.Sequence + 1 == request.Sequence)
            {
                subscriptor.Status = InvitationState.Joined.ToString();
                subscriptor.Sequence = request.Sequence;
                await invitationEventsRepository.Complete();
                return true;
            }
            else if (subscriptor.Sequence + 1 < request.Sequence) return false;
            else if (subscriptor.Sequence + 1 > request.Sequence) return true;
            
            return false;
        }
    }
}
