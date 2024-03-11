using InvitationCommandService.Domain.Model;
using MediatR;

namespace InvitationCommandService.Application.CommandHandler.Send
{
    public record SendInvitationCommand(int accountId,
            int subscriptionId,
            int userId,
            int memberId,
            List<PermissionsModel> Permissions) : IRequest<int>;

}
