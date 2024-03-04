using Grpc.Core;
using InvitationQueryService;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Presentation.Exceptions;
using InvitationQueryTest.QuerySide.GetAllPermissions;
using MediatR;

namespace InvitationQueryService.Presentation.Services
{
    public class PermissionsService:Permissions.PermissionsBase
    {
        private readonly IMediator mediator;

        public PermissionsService(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public override async Task<ManyPermission> GetAll(PermissionPage request, ServerCallContext context)
        {
            if (request.NumberPage<1)
            {
                throw new BadPageException("Number Page must be positive.");
            }
            var query = new GetAllPermissionsQuery(request.NumberPage);
            List<PermissionEntity> data = await mediator.Send(query);

            ManyPermission permission = new ManyPermission();
            foreach(var d in data)
            {
                permission.Permission.Add(new Permission
                {
                    Id = d.Id,
                    Name = d.Name
                });
            }
            return permission;
        }
    }
}
