
using InvitationQueryService.Domain;
using InvitationQueryService.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace InvitationQueryTest.Database
{
    public class DatabaseHelper
    {
        public static void AddSubscriptionAndSubscriptor(WebApplicationFactory<Program> factory)
        {
            var scope = factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            for(int i=2; i<20; i++)
            {
                database.Subscriptions.AddRange(
                new InvitationQueryService.Domain.Entities.SubscriptionsEntity
                {
                    AccountId =i
                }
                );
            }
            int counter = 1;
            for(int i=1; i<20; i++)
            {
                for( int j=1; j<20; j++)
                {
                    database.Subscriptors.Add(
                        new InvitationQueryService.Domain.Entities.SubscriptorEntity
                        {
                            Sequence = counter,
                            SubscriptionId = j,
                            SubscriptorAccountId = i,
                            Status = (j%3==0)?InvitationState.Pending.ToString()
                                                : (j % 3 == 0)? InvitationState.Out.ToString()
                                                : InvitationState.Joined.ToString()
                        });
                        counter++;
                    database.SubscriptionPermissions.Add(
                        new InvitationQueryService.Domain.Entities.SubscriptorPermissionsEntity
                        {
                            PermissionId = (j % 2 == 0) ? 1 : 2,
                            SubscriptionId = j,
                            SubscriptorId = i
                        });
                }
            }
            database.SaveChanges();
        }
    }
}
