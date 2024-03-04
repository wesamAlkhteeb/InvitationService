using InvitationQueryService.Domain.Entities;
using MediatR;

namespace InvitationQueryTest.QuerySide.GetAllSubscriptor
{
    public class GetAllSubscriptorQueryHandler : IRequestHandler<GetAllSubscriptorQuery, List<SubscriptorEntity>>
    {
        public Task<List<SubscriptorEntity>> Handle(GetAllSubscriptorQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    
}
