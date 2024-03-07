using InvitationQueryService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryTest.QuerySide.GetAllSubscriptionForSubscriptor
{
    public record GetAllSubscriptionForSubscriptorQuery(int page , int userId) : IRequest<List<SubscriptionsEntity>>;
}
