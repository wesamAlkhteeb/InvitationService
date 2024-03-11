using MediatR;

namespace InvitationQueryService.Application.QuerySide.CheckPermission
{
    public record CheckPermissionQuery(int id):IRequest<string>;
}
