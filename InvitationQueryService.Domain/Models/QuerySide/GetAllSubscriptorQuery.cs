﻿using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Models.QuerySide
{
    public record GetAllSubscriptorQuery(int page , int subscriptionId) : IRequest<List<UsersInSubscriptionResponseModel>>;

    
}
