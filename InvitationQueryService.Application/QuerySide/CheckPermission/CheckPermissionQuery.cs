using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Application.QuerySide.CheckPermission
{
    public record CheckPermissionQuery(int id):IRequest<string>;
}
