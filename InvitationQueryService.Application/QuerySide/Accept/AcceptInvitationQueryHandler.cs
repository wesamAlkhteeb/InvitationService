using InvitationQueryService.Application.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Application.QuerySide.Accept
{
    public class AcceptInvitationQueryHandler : IRequestHandler<AcceptInvitationQuery, bool>
    {
        private readonly IInvitationEventsRepository invitationEventsRepository;

        public AcceptInvitationQueryHandler(IInvitationEventsRepository invitationEventsRepository)
        {
            this.invitationEventsRepository = invitationEventsRepository;
        }
        public async Task<bool> Handle(AcceptInvitationQuery request, CancellationToken cancellationToken)
        {
            int realSequence = await invitationEventsRepository
                .GetSequence(request.Data.MemberId, request.Data.SubscriptionId);
            if (realSequence + 1 < request.Sequence) return false;
            if (realSequence + 1 > request.Sequence) return true;
            if (realSequence + 1 == request.Sequence)
            {
                await invitationEventsRepository.AcceptInvitation(request);
                return true;
            }
            return false;
        }
    }
}
