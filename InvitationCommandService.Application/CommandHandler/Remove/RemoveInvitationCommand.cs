using MediatR;

namespace InvitationQueryService.Application.CommandHandler.Remove
{
    public record RemoveInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId) : IRequest<int>;
}
