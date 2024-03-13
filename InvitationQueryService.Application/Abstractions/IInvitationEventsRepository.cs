
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Models.QuerySideServiceBus;

namespace InvitationQueryService.Application.Abstractions
{
    public interface IInvitationEventsRepository
    {
        Task<SubscriptorEntity?> GetSubscriptor(int subscriptorAccountId, int subscriptionId);
        Task SendInvitation(SendInvitationQuery sendInvitationQuery);
        Task JoinInvitation(JoinInvitationQuery joinInvitationQuery);
        Task ChangePermissions(ChangePermissionsInvitationQuery changePermissionsInvitationQuery ,int subscriptorId);
        Task Complete();
    }
}
