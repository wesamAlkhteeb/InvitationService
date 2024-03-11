using InvitationQueryService.Domain.Entities;
using MediatR;

namespace InvitationQueryService.QuerySide.GetAllSubscriptionForOwner
{
    public record GetAllSubscriptionForOwnerQuery(int page, int OwnerId):IRequest<List<SubscriptionsEntity>>;
}
