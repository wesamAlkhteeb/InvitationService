using InvitationCommandService.Domain.Model;
using MediatR;

namespace InvitationCommandService.Application.CommandHandler.ChangePermissions
{
    public record ChangePermissionInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId,
            List<PermissionsModel> Permissions) : IRequest<int>;
}
