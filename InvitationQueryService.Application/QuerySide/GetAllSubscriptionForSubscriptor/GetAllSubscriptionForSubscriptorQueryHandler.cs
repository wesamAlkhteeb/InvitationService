using InvitationQueryService.Domain.Entities;
using MediatR;

namespace InvitationQueryTest.QuerySide.GetAllSubscriptionForSubscriptor
{
    public class GetAllSubscriptionForSubscriptorQueryHandler : IRequestHandler<GetAllSubscriptionForSubscriptorQuery, List<SubscriptionsEntity>>
    {
        public Task<List<SubscriptionsEntity>> Handle(GetAllSubscriptionForSubscriptorQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
