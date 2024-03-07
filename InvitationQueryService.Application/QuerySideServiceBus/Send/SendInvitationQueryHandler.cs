using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InvitationQueryService.Application.QuerySideServiceBus.Send
{
    public class SendInvitationQueryHandler : IRequestHandler<SendInvitationQuery, bool>
    {
        private readonly IInvitationEventsRepository invitationEventsRepository;
        private readonly ILogger<SendInvitationQueryHandler> logger;

        public SendInvitationQueryHandler(IInvitationEventsRepository invitationEventsRepository, 
            ILogger<SendInvitationQueryHandler> logger)
        {
            this.invitationEventsRepository = invitationEventsRepository;
            this.logger = logger;
        }
        public async Task<bool> Handle(SendInvitationQuery request, CancellationToken cancellationToken)
        {
            SubscriptorEntity? subscriptor = await invitationEventsRepository
                .GetSubscriptor(request.Data.Info.MemberId, request.Data.Info.SubscriptionId);

            if (subscriptor == null)
            {
                await invitationEventsRepository.SendInvitation(request);
                return true;
            }
            else if (subscriptor.Sequence == request.Sequence)
            {
                subscriptor.Status = InvitationState.Pending.ToString();
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
