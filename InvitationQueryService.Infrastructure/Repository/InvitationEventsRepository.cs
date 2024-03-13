using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Application.QuerySideServiceBus.Accept;
using InvitationQueryService.Application.QuerySideServiceBus.Cancel;
using InvitationQueryService.Application.QuerySideServiceBus.ChangePermission;
using InvitationQueryService.Application.QuerySideServiceBus.Join;
using InvitationQueryService.Application.QuerySideServiceBus.Leave;
using InvitationQueryService.Application.QuerySideServiceBus.Reject;
using InvitationQueryService.Application.QuerySideServiceBus.Remove;
using InvitationQueryService.Application.QuerySideServiceBus.Send;
using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Domain.Exceptions;
using InvitationQueryService.Domain.Models;
using InvitationQueryService.Infrastructure.Database;
using InvitationQueryService.Models.QuerySideServiceBus;
using Microsoft.EntityFrameworkCore;

namespace InvitationCommandService.Infrastructure.Repository
{
    public class InvitationEventsRepository : IInvitationEventsRepository
    {
        private readonly InvitationDbContext database;

        public InvitationEventsRepository(InvitationDbContext database)
        {
            this.database = database;
        }

        public async Task SendInvitation(SendInvitationQuery sendInvitationQuery)
        {
            SubscriptorEntity subscriptorEntity = new SubscriptorEntity
            {
                Sequence = sendInvitationQuery.Sequence,
                Status = InvitationState.Pending.ToString(),
                SubscriptionId = sendInvitationQuery.Data.Info.SubscriptionId,
                SubscriptorAccountId = sendInvitationQuery.Data.Info.MemberId
            };
            await database.Subscriptors.AddAsync(subscriptorEntity);
            await database.SaveChangesAsync();
            AddPermissionsForSubscriptor(
                sendInvitationQuery.Data.Permissions,
                sendInvitationQuery.Data.Info.SubscriptionId,
                subscriptorEntity.Id
                );
            await database.SaveChangesAsync();
        }

        public async Task ChangePermissions(ChangePermissionsInvitationQuery changePermissionsInvitationQuery , int subscriptorId)
        {
            
            List<SubscriptorPermissionsEntity> permissionsEntity =
                await database.SubscriptionPermissions
                .Where(
                    x =>
                        x.SubscriptionId == changePermissionsInvitationQuery.Data.Info.SubscriptionId &&
                        x.SubscriptorId == subscriptorId
                        ).ToListAsync();
            database.RemoveRange(permissionsEntity);
            AddPermissionsForSubscriptor(
                changePermissionsInvitationQuery.Data.Permissions,
                changePermissionsInvitationQuery.Data.Info.SubscriptionId,
                subscriptorId
                );
            await database.SaveChangesAsync();
        }

        public async Task JoinInvitation(JoinInvitationQuery joinInvitationQuery)
        {
            
            SubscriptorEntity subscriptorEntity = new SubscriptorEntity
            {
                Sequence = joinInvitationQuery.Sequence,
                Status = InvitationState.Joined.ToString(),
                SubscriptionId = joinInvitationQuery.Data.Info.SubscriptionId,
                SubscriptorAccountId = joinInvitationQuery.Data.Info.MemberId
            };
            await database.Subscriptors.AddAsync(subscriptorEntity);
            await database.SaveChangesAsync();
            AddPermissionsForSubscriptor(
                joinInvitationQuery.Data.Permissions,
                joinInvitationQuery.Data.Info.SubscriptionId,
                subscriptorEntity.Id
                );
            await database.SaveChangesAsync();
        }

        public async Task<SubscriptorEntity?> GetSubscriptor(int subscriptorAccountId, int subscriptionId)
        {
            return await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == subscriptorAccountId
                            && x.SubscriptionId == subscriptionId)
                .FirstOrDefaultAsync();
        }

        private void AddPermissionsForSubscriptor(List<PermissionModel> permissions, int subscriptionId, int subscriptorId)
        {
            foreach (var permission in permissions)
            {
                database.SubscriptionPermissions.Add(new SubscriptorPermissionsEntity
                {
                    PermissionId = permission.Id,
                    SubscriptionId = subscriptionId,
                    SubscriptorId = subscriptorId,
                });
            }
        }

        public async Task Complete()
        {
            await database.SaveChangesAsync();
        }
    }
}
