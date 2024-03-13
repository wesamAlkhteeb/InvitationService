using MediatR;

namespace InvitationQueryService.Models.QuerySide
{
    public record CheckPermissionQuery(int id):IRequest<string>;
}
