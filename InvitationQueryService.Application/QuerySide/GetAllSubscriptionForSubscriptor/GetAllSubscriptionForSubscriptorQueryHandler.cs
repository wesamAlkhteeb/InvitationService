using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.QuerySide.GetAllSubscriptionForSubscriptor;
using MediatR;

namespace InvitationQueryTest.QuerySide.GetAllSubscriptionForSubscriptor
{
    public class GetAllSubscriptionForSubscriptorQueryHandler : IRequestHandler<GetAllSubscriptionForSubscriptorQuery, List<SubscriptionsEntity>>
    {
        private readonly ISubscriptorRepository subscriptorRepository;

        public GetAllSubscriptionForSubscriptorQueryHandler(ISubscriptorRepository subscriptorRepository)
        {
            this.subscriptorRepository = subscriptorRepository;
        }
        public async Task<List<SubscriptionsEntity>> Handle(GetAllSubscriptionForSubscriptorQuery request, CancellationToken cancellationToken)
        {
            return await subscriptorRepository.GetSubscriptionForUser(request.page, request.userId);
        }
    }
}
