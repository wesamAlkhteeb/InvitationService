using InvitationQueryService.Domain.Entities;
using MediatR;

namespace InvitationQueryService.Models.QuerySide
{
    public record GetAllSubscriptionForSubscriptorQuery(int page , int userId) : IRequest<List<SubscriptionsEntity>>;
}
