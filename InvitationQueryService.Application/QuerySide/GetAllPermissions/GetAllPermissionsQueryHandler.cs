﻿using InvitationQueryService.Application.Abstractions;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Models.QuerySide;
using MediatR;

namespace InvitationQueryService.QuerySide.GetAllPermissions
{
    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, List<PermissionEntity>>
    {
        private readonly IPermissionRepository permissionRepository;

        public GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository)
        {
            this.permissionRepository = permissionRepository;
        }
        public Task<List<PermissionEntity>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            return permissionRepository.GetAllPermissions(request.page);
        }
    }
}
