using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

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
            SubscriptorEntity? subscriptor = await invitationEventsRepository
                .GetSubscriptor(request.Data.MemberId, request.Data.SubscriptionId);

            if (subscriptor == null)
            {
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
            return false;
        }
    }
}
