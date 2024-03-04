using InvitationQueryService.Domain.Entities;
using MediatR;

namespace InvitationQueryTest.QuerySide.GetAllPermissions
{
    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, List<PermissionEntity>>
    {
        public Task<List<PermissionEntity>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
