using MediatR;

namespace InvitationCommandService.Domain.Model.Command
{
    public record RemoveInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId) : IRequest<int>;
}
