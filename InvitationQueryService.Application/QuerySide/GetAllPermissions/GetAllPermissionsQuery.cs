using InvitationQueryService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryTest.QuerySide.GetAllPermissions
{
    public record GetAllPermissionsQuery(int page):IRequest<List<PermissionEntity>>;
}
