using MediatR;

namespace InvitationCommandService.Domain.Model.Command
{
    public record CancelInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId) : IRequest<int>;
}
