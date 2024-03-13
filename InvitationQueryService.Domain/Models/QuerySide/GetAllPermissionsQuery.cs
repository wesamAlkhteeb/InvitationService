using InvitationQueryService.Domain.Entities;
using MediatR;

namespace InvitationQueryService.Models.QuerySide
{
    public record GetAllPermissionsQuery(int page):IRequest<List<PermissionEntity>>;
}
