using InvitationQueryService.Domain.Entities;
using MediatR;

namespace InvitationQueryService.Models.QuerySide
{
    public record GetAllSubscriptionForOwnerQuery(int page, int OwnerId):IRequest<List<SubscriptionsEntity>>;
}
