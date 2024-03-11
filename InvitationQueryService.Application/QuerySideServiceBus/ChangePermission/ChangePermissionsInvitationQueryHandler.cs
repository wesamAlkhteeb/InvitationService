using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InvitationQueryService.Application.QuerySideServiceBus.ChangePermission
{
    public class ChangePermissionsInvitationQueryHandler : IRequestHandler<ChangePermissionsInvitationQuery, bool>
    {
        private readonly IInvitationEventsRepository invitationEventsRepository;
        private readonly Logger<ChangePermissionsInvitationQueryHandler> logger;

        public ChangePermissionsInvitationQueryHandler(IInvitationEventsRepository invitationEventsRepository,Logger<ChangePermissionsInvitationQueryHandler> logger)
        {
            this.invitationEventsRepository = invitationEventsRepository;
            this.logger = logger;
        }
        public async Task<bool> Handle(ChangePermissionsInvitationQuery request, CancellationToken cancellationToken)
        {
            SubscriptorEntity? subscriptor = await invitationEventsRepository
                .GetSubscriptor(request.Data.Info.MemberId, request.Data.Info.SubscriptionId);

            if (subscriptor == null)
            {
                logger.LogWarning("I don't have send event record in database and receive accept Event");
                return false;
            }
            else if (subscriptor.Sequence == request.Sequence)
            {
                await invitationEventsRepository.ChangePermissions(request, subscriptor.Id);
                subscriptor.Sequence = request.Sequence;
                await invitationEventsRepository.Complete();
                return true;
            }
            else if (subscriptor.Sequence + 1 < request.Sequence) return false;
            else if (subscriptor.Sequence + 1 > request.Sequence) return true;
            logger.LogWarning("there are handle error");
            return false;
        }
    }
}
