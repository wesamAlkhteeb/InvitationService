using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Models.QuerySide;
using MediatR;

namespace InvitationQueryService.Application.QuerySide.CheckPermission
{
    public class CheckPermissionQueryHandler : IRequestHandler<CheckPermissionQuery, string>
    {
        private readonly IPermissionRepository permissionRepository;

        public CheckPermissionQueryHandler(IPermissionRepository permissionRepository)
        {
            this.permissionRepository = permissionRepository;
        }
        public Task<string> Handle(CheckPermissionQuery request, CancellationToken cancellationToken)
        {
            return permissionRepository.IsExistsPermission(request.id);
        }
    }
}
