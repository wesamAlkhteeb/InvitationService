using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Domain.Models.Response;
using InvitationQueryService.Models.QuerySide;
using MediatR;

namespace InvitationQueryService.QuerySide.GetAllSubscriptor
{
    public class GetAllSubscriptorsInSubscriptionQueryHandler : IRequestHandler<GetAllSubscriptorQuery, List<UsersInSubscriptionResponseModel>>
    {
        private readonly ISubscriptorRepository subscriptorRepository;

        public GetAllSubscriptorsInSubscriptionQueryHandler(ISubscriptorRepository subscriptorRepository)
        {
            this.subscriptorRepository = subscriptorRepository;
        }
        public async Task<List<UsersInSubscriptionResponseModel>> Handle(GetAllSubscriptorQuery request, CancellationToken cancellationToken)
        {
            return await subscriptorRepository.GetUserinSubscription(request.page, request.subscriptionId);
        }
    }   
}
