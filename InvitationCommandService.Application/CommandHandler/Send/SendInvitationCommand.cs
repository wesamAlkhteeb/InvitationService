using InvitationCommandService.Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationCommandService.Application.CommandHandler.Send
{
    public record SendInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId,
            List<PermissionsModel> Permissions) : IRequest<int>;
    
}
