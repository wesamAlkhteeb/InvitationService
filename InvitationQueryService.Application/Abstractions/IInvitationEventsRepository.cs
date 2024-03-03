using InvitationQueryService.Application.QuerySide.Accept;
using InvitationQueryService.Application.QuerySide.Cancel;
using InvitationQueryService.Application.QuerySide.ChangePermission;
using InvitationQueryService.Application.QuerySide.Join;
using InvitationQueryService.Application.QuerySide.Leave;
using InvitationQueryService.Application.QuerySide.Reject;
using InvitationQueryService.Application.QuerySide.Remove;
using InvitationQueryService.Application.QuerySide.Send;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Application.Abstractions
{
    public interface IInvitationEventsRepository
    {
        Task<int> GetSequence(int subscriptorAccountId,int subscriptionId);
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
