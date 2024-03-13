using MediatR;

namespace InvitationCommandService.Domain.Model.Command
{
    public record RejectInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId) : IRequest<int>;
}
