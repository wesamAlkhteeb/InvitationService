using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Domain.Models.Response;
using InvitationQueryService.QuerySide.GetAllSubscriptionForSubscriptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Application.Abstractions
{
    public interface ISubscriptorRepository
    {
        public Task<List<SubscriptionsEntity>> GetSubscriptionForOwner(int page, int ownerId);
        public Task<List<SubscriptionsEntity>> GetSubscriptionForUser(int page , int userId);
        public Task<List<UsersInSubscriptionResponseModel>> GetUserinSubscription(int page, int subscriptionId);
    }
}
