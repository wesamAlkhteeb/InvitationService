using MediatR;

namespace InvitationCommandService.Application.CommandHandler.Reject
{
    public record RejectInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId) : IRequest<int>;
}
