
using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace InvitationQueryTest.DatabaseQuery
{
    public class DatabaseQueryHelper
    {
        public static async Task<int> AddSubscription(
            WebApplicationFactory<Program> factory,
                int accountId
            )
        {
            IServiceScope scope = factory.Services.CreateScope();
            InvitationDbContext database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();

            SubscriptionsEntity subscription = new SubscriptionsEntity
            {
                AccountId = accountId,
                Type = SubscriptionType.A.ToString()
            };
            await database.Subscriptions.AddAsync(subscription);
            await database.SaveChangesAsync();
            return subscription.Id;
        }

        public static async Task<int> AddSubscriptor(
            WebApplicationFactory<Program> factory,
                int sequence,
                int subscriptionId,
                int memberId
            )
        {
            var scope = factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();

            SubscriptorEntity subscriptor = new SubscriptorEntity
            {
                Status = InvitationState.Out.ToString(),
                Sequence = sequence,
                SubscriptionId = subscriptionId,
                SubscriptorAccountId = memberId
            };
            await database.Subscriptors.AddAsync(subscriptor);
            await database.SaveChangesAsync();
            return subscriptor.Id;
        }

        public static async Task<int> AddPermissions(
            WebApplicationFactory<Program> factory,
                string name
            )
        {
            var scope = factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();

            PermissionEntity permission = new PermissionEntity
            {
                Name = name
            };
            await database.Permissions.AddAsync(permission);
            await database.SaveChangesAsync();
            return permission.Id;
        }

        public static async Task<int> AddSubscriptorPermission(
            WebApplicationFactory<Program> factory,
                int subscriptorId,
                int subscriptionId,
                int permissionId
            )
        {
            var scope = factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();

            SubscriptorPermissionsEntity subscriptorPermissions = new SubscriptorPermissionsEntity
            {
                PermissionId = permissionId,
                SubscriptionId = subscriptionId,
                SubscriptorId = subscriptorId
            };
            await database.SubscriptionPermissions.AddAsync(subscriptorPermissions);
            await database.SaveChangesAsync();
            return subscriptorPermissions.Id;
        }
    }
}
