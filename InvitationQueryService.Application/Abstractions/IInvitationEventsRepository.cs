﻿using InvitationQueryService.Application.QuerySideServiceBus.ChangePermission;
using InvitationQueryService.Application.QuerySideServiceBus.Join;
using InvitationQueryService.Application.QuerySideServiceBus.Send;
using InvitationQueryService.Domain.Entities;

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
