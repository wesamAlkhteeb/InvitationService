using MediatR;

namespace InvitationQueryTest.QuerySide.GetStatus
{
    public class GetStatusQueryHandler : IRequestHandler<GetStatusQuery, string>
    {
        public Task<string> Handle(GetStatusQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
