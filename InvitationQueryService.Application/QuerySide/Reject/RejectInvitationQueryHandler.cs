using InvitationQueryService.Application.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Application.QuerySide.Reject
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
