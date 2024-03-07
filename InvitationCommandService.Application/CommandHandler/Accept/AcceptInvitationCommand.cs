using MediatR;

namespace InvitationQueryService.Application.CommandHandler.Accept
{
    public record AcceptInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId) : IRequest<int>;
}
