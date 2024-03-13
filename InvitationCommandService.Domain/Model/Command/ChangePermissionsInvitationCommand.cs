using InvitationCommandService.Domain.Model;
using MediatR;

namespace InvitationCommandService.Domain.Model.Command
{
    public record ChangePermissionInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId,
            List<PermissionsModel> Permissions) : IRequest<int>;
}
