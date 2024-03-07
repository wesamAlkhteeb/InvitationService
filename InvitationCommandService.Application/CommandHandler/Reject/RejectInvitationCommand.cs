using MediatR;

namespace InvitationQueryService.Application.CommandHandler.Reject
{
    public record RejectInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId) : IRequest<int>;
}
