using InvitationQueryService.Application.QuerySideServiceBus.Accept;
using InvitationQueryService.Application.QuerySideServiceBus.Cancel;
using InvitationQueryService.Application.QuerySideServiceBus.ChangePermission;
using InvitationQueryService.Application.QuerySideServiceBus.Join;
using InvitationQueryService.Application.QuerySideServiceBus.Leave;
using InvitationQueryService.Application.QuerySideServiceBus.Reject;
using InvitationQueryService.Application.QuerySideServiceBus.Remove;
using InvitationQueryService.Application.QuerySideServiceBus.Send;

namespace InvitationQueryService.Application.Abstractions
{
    public interface IInvitationEventsRepository
    {
        Task<int> GetSequence(int subscriptorAccountId, int subscriptionId);
        Task SendInvitation(SendInvitationQuery sendInvitationQuery);
        Task RejectInvitation(RejectInvitationQuery rejectInvitationQuery);
        Task AcceptInvitation(AcceptInvitationQuery acceptInvitationQuery);
        Task RemoveInvitation(RemoveInvitationQuery removeInvitationQuery);
        Task CancelInvitation(CancelInvitationQuery cancelInvitationQuery);
        Task LeaveInvitation(LeaveInvitationQuery leaveInvitationQuery);
        Task JoinInvitation(JoinInvitationQuery joinInvitationQuery);
        Task ChangePermissions(ChangePermissionsInvitationQuery changePermissionsInvitationQuery);
    }
}
