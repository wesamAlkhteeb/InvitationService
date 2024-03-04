using InvitationQueryService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryTest.QuerySide.GetAllSubscriptor
{
    public record GetAllSubscriptorQuery(int page) : IRequest<List<SubscriptorEntity>>;

    
}
