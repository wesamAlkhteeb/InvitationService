using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Domain.Models.Response;
using InvitationQueryService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace InvitationCommandService.Infrastructure.Repository
{
    public class SubscriptorRepository : ISubscriptorRepository
    {
        private readonly InvitationDbContext database;

        public SubscriptorRepository(InvitationDbContext database)
        {
            this.database = database;
        }

        public async Task<List<SubscriptionsEntity>> GetSubscriptionForOwner(int page, int ownerId)
        {
            int skip = (page - 1) * Constants.COUNT_ITEM_IN_PAGE;
            return await database.Subscriptions
                .Where(x=>x.AccountId == ownerId)
                .Skip(skip).Take(Constants.COUNT_ITEM_IN_PAGE)
                .ToListAsync();
        }

        public async Task<List<SubscriptionsEntity>> GetSubscriptionForUser(int page, int userId)
        {
            int skip = (page - 1) * Constants.COUNT_ITEM_IN_PAGE;
            return await database.Subscriptors
                    .Where( x => x.SubscriptorAccountId == userId )
                    .Join(
                        database.Subscriptions,
                        subscriptor => subscriptor.SubscriptionId,
                        subscription => subscription.Id,
                        (subscriptor, subscription) => subscription
                    ).ToListAsync();
        }

        public async Task<List<UsersInSubscriptionResponseModel>> GetUserinSubscription(int page, int subscriptionId)
        {
            int skip = (page - 1) * Constants.COUNT_ITEM_IN_PAGE;
            return await database.Subscriptors
                .Where(x => x.SubscriptionId == subscriptionId)
                .Skip(skip).Take(Constants.COUNT_ITEM_IN_PAGE)
                .Select(x=> new UsersInSubscriptionResponseModel
                {
                    Id = x.Id,
                    Status = x.Status,
                    UserId = x.SubscriptorAccountId
                })
                .ToListAsync();
        }

    }
}
