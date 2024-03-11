using InvitationQueryService.Domain.Entities;
using MediatR;

namespace InvitationQueryService.QuerySide.GetAllPermissions
{
    public record GetAllPermissionsQuery(int page):IRequest<List<PermissionEntity>>;
}
