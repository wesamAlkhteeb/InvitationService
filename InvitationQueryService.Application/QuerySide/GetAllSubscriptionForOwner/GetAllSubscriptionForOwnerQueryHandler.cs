using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain.Entities;
using MediatR;

namespace InvitationQueryService.QuerySide.GetAllSubscriptionForOwner
{
    public class GetAllSubscriptionForOwnerQueryHandler : IRequestHandler<GetAllSubscriptionForOwnerQuery, List<SubscriptionsEntity>>
    {
        private readonly ISubscriptorRepository subscriptorRepository;

        public GetAllSubscriptionForOwnerQueryHandler(ISubscriptorRepository subscriptorRepository)
        {
            this.subscriptorRepository = subscriptorRepository;
        }
        public async Task<List<SubscriptionsEntity>> Handle(GetAllSubscriptionForOwnerQuery request, CancellationToken cancellationToken)
        {
            return await subscriptorRepository.GetSubscriptionForOwner(request.page , request.OwnerId);
        }
    }
}
