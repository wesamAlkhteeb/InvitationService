using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationCommandService.Application.CommandHandler.Reject
{
    public record RejectInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId) : IRequest<int>;
}
