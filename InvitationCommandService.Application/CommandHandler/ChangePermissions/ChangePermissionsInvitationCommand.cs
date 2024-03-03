using InvitationCommandService.Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationCommandService.Application.CommandHandler.ChangePermissions
{
    public record ChangePermissionInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId,
            List<PermissionsModel> Permissions) : IRequest<int>;
}
