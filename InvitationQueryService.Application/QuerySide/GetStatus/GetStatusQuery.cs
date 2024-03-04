using InvitationQueryService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryTest.QuerySide.GetStatus
{
    public record GetStatusQuery(int userId, int ownerId) : IRequest<string>;
}
