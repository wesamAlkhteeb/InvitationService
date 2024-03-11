using Grpc.Core;
using InvitationQueryService;
using InvitationQueryService.Application.QuerySide.CheckPermission;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Presentation.Exceptions;
using InvitationQueryService.QuerySide.GetAllPermissions;
using MediatR;

namespace InvitationCommandService.Presentation.Services
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
        public override async Task<Response> Check(PermissionId request, ServerCallContext context)
        {
            if (request.Id < 1)
            {
                throw new BadPageException("Number Page must be positive.");
            }
            var query = new CheckPermissionQuery(request.Id);
            string name = await mediator.Send(query);
            return new Response
            {
                Message = $"Permission {name} is Exists"
            };
        }
    }
}
