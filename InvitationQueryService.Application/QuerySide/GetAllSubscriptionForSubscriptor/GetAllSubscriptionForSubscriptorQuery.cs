using InvitationQueryService.Domain.Entities;
using MediatR;

namespace InvitationQueryService.QuerySide.GetAllSubscriptionForSubscriptor
{
    public record GetAllSubscriptionForSubscriptorQuery(int page , int userId) : IRequest<List<SubscriptionsEntity>>;
}
