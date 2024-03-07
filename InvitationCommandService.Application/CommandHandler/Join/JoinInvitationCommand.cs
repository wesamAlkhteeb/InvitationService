using InvitationQueryService.Domain.Model;
using MediatR;

namespace InvitationQueryService.Application.CommandHandler.Join
{
    public record JoinInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId,
            List<PermissionsModel> Permissions) : IRequest<int>;
}
