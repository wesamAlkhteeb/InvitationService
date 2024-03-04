﻿using InvitationQueryService.Application.Abstractions;
using MediatR;

namespace InvitationQueryService.Application.QuerySideServiceBus.ChangePermission
{
    internal class ChangePermissionsInvitationQueryHandler : IRequestHandler<ChangePermissionsInvitationQuery, bool>
    {
        private readonly IInvitationEventsRepository invitationEventsRepository;

        public ChangePermissionsInvitationQueryHandler(IInvitationEventsRepository invitationEventsRepository)
        {
            this.invitationEventsRepository = invitationEventsRepository;
        }
        public async Task<bool> Handle(ChangePermissionsInvitationQuery request, CancellationToken cancellationToken)
        {
            int realSequence = await invitationEventsRepository
                    .GetSequence(request.Data.Info.MemberId, request.Data.Info.SubscriptionId);
            if (realSequence + 1 < request.Sequence) return false;
            if (realSequence + 1 > request.Sequence) return true;
            if (realSequence + 1 == request.Sequence)
            {
                await invitationEventsRepository.ChangePermissions(request);
                return true;
            }
            return false;
        }
    }
}