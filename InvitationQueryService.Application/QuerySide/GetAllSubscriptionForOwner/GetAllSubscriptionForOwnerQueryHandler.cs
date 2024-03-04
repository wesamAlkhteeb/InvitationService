using InvitationQueryService.Domain.Entities;
using MediatR;

namespace InvitationQueryTest.QuerySide.GetAllSubscriptionForOwner
{
    public class GetAllSubscriptionForOwnerQueryHandler : IRequestHandler<GetAllSubscriptionForOwnerQuery, List<SubscriptionsEntity>>
    {
        public Task<List<SubscriptionsEntity>> Handle(GetAllSubscriptionForOwnerQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
