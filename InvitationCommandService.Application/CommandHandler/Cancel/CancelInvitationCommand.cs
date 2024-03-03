using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationCommandService.Application.CommandHandler.Cancel
{
    public record CancelInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId) : IRequest<int>;
}
