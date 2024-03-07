using MediatR;

namespace InvitationQueryService.Application.CommandHandler.Leave
{
    public record LeaveInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId) : IRequest<int>;
}
