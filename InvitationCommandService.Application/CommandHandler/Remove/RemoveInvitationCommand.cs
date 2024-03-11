using MediatR;

namespace InvitationCommandService.Application.CommandHandler.Remove
{
    public record RemoveInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId) : IRequest<int>;
}
