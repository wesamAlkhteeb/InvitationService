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
using Microsoft.EntityFrameworkCore;

namespace InvitationQueryService.Infrastructure.Repository
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
            SubscriptorEntity? subscriptor =
                await GetSubscriptor(sendInvitationQuery.Data.Info.MemberId, sendInvitationQuery.Data.Info.SubscriptionId);
            if (subscriptor is not null)
            {
                subscriptor.Status = InvitationState.Pending.ToString();
                await database.SaveChangesAsync();
            }
            else
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
        }

        public async Task AcceptInvitation(AcceptInvitationQuery acceptInvitationQuery)
        {
            SubscriptorEntity? subscriptor =
                await GetSubscriptor(acceptInvitationQuery.Data.MemberId, acceptInvitationQuery.Data.SubscriptionId);
            if (subscriptor is null)
            {
                throw new OperationsOrderException("");
            }
            else
            {
                subscriptor.Status = InvitationState.Joined.ToString();
                subscriptor.Sequence = acceptInvitationQuery.Sequence;
                await database.SaveChangesAsync();
            }
        }

        public async Task CancelInvitation(CancelInvitationQuery cancelInvitationQuery)
        {
            SubscriptorEntity? subscriptor =
                await GetSubscriptor(cancelInvitationQuery.Data.MemberId, cancelInvitationQuery.Data.SubscriptionId);
            if (subscriptor is null)
            {
                throw new OperationsOrderException("");
            }
            else
            {
                subscriptor.Status = InvitationState.Out.ToString();
                subscriptor.Sequence = cancelInvitationQuery.Sequence;
                await database.SaveChangesAsync();
            }
        }

        public async Task ChangePermissions(ChangePermissionsInvitationQuery changePermissionsInvitationQuery)
        {
            SubscriptorEntity? subscriptor =
                await GetSubscriptor(changePermissionsInvitationQuery.Data.Info.MemberId, changePermissionsInvitationQuery.Data.Info.SubscriptionId);
            if (subscriptor is null)
            {

                throw new OperationsOrderException("");
            }
            else
            {
                List<SubscriptorPermissionsEntity> permissionsEntity =
                    await database.SubscriptionPermissions
                    .Where(
                        x =>
                            x.SubscriptionId == changePermissionsInvitationQuery.Data.Info.SubscriptionId &&
                            x.SubscriptorId == subscriptor.Id
                            ).ToListAsync();
                database.RemoveRange(permissionsEntity);
                subscriptor.Sequence = changePermissionsInvitationQuery.Sequence;
                AddPermissionsForSubscriptor(
                    changePermissionsInvitationQuery.Data.Permissions,
                    changePermissionsInvitationQuery.Data.Info.SubscriptionId,
                    subscriptor.Id
                    );
                await database.SaveChangesAsync();
            }
        }

        public async Task<int> GetSequence(int subscriptorAccountId, int subscriptionId)
        {
            return await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == subscriptorAccountId && x.SubscriptionId == subscriptionId)
                .Select(x => x.Sequence).FirstOrDefaultAsync();

        }

        public async Task JoinInvitation(JoinInvitationQuery joinInvitationQuery)
        {
            SubscriptorEntity? subscriptor =
                await GetSubscriptor(joinInvitationQuery.Data.Info.MemberId, joinInvitationQuery.Data.Info.SubscriptionId);
            if (subscriptor is not null)
            {
                subscriptor.Status = InvitationState.Joined.ToString();
                await database.SaveChangesAsync();
            }
            else
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
        }

        public async Task LeaveInvitation(LeaveInvitationQuery leaveInvitationQuery)
        {
            SubscriptorEntity? subscriptor =
                await GetSubscriptor(leaveInvitationQuery.Data.MemberId, leaveInvitationQuery.Data.SubscriptionId);
            if (subscriptor is null)
            {
                throw new OperationsOrderException("");
            }
            else
            {
                subscriptor.Status = InvitationState.Out.ToString();
                subscriptor.Sequence = leaveInvitationQuery.Sequence;
                await database.SaveChangesAsync();
            }
        }

        public async Task RejectInvitation(RejectInvitationQuery rejectInvitationQuery)
        {
            SubscriptorEntity? subscriptor =
                await GetSubscriptor(rejectInvitationQuery.Data.MemberId, rejectInvitationQuery.Data.SubscriptionId);
            if (subscriptor is null)
            {
                throw new OperationsOrderException("");
            }
            else
            {
                subscriptor.Status = InvitationState.Out.ToString();
                subscriptor.Sequence = rejectInvitationQuery.Sequence;
                await database.SaveChangesAsync();
            }
            throw new NotImplementedException();
        }

        public async Task RemoveInvitation(RemoveInvitationQuery removeInvitationQuery)
        {
            SubscriptorEntity? subscriptor =
                await GetSubscriptor(removeInvitationQuery.Data.MemberId, removeInvitationQuery.Data.SubscriptionId);
            if (subscriptor is null)
            {
                throw new OperationsOrderException("");
            }
            else
            {
                database.Remove(subscriptor);
                await database.SaveChangesAsync();
            }
        }


        private async Task<SubscriptorEntity?> GetSubscriptor(int subscriptorAccountId, int subscriptionId)
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
    }
}
