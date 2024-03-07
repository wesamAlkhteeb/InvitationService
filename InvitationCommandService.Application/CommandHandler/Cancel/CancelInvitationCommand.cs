using MediatR;

namespace InvitationQueryService.Application.CommandHandler.Cancel
{
    public record CancelInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId) : IRequest<int>;
}
