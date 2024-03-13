using Grpc.Core;
using InvitationQueryService;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Domain.Models.Response;
using InvitationQueryService.Models.QuerySide;
using InvitationQueryService.Presentation.Exceptions;
using MediatR;

namespace InvitationCommandService.Presentation.Services
{
    public class SubsctiptionsService : Subscriptions.SubscriptionsBase
    {
        private readonly IMediator mediator;

        public SubsctiptionsService(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public override async Task<ManyOwnerSubscriptionReuslt> GetAllSubscriptionForOwner(OwnerSubscription request, ServerCallContext context)
        {
            if (request.Page < 1 || request.OwnerId < 1)
            {
                throw new BadPageException("Number Page must be positive.");
            }
            var query = new GetAllSubscriptionForOwnerQuery(request.Page, request.OwnerId);
            List<SubscriptionsEntity> data = await mediator.Send(query);
            ManyOwnerSubscriptionReuslt subscrption = new ManyOwnerSubscriptionReuslt();
            foreach (var d in data)
            {
                subscrption.OwnerSubscriptionReuslt.Add(new OwnerSubscriptionReuslt
                {
                    Id = d.Id,
                    Type = d.Type
                });
            }
            return subscrption;
        }

        public override async Task<ManyUserSubscriptorReuslt> GetAllSubscriptionForSubscriptor(UserSubscription request, ServerCallContext context)
        {
            if (request.Page < 1 || request.UserId < 1)
            {
                throw new BadPageException("Number Page must be positive.");
            }
            var query = new GetAllSubscriptionForSubscriptorQuery(request.Page, request.UserId);
            List<SubscriptionsEntity> data = await mediator.Send(query);
            ManyUserSubscriptorReuslt subscrption = new ManyUserSubscriptorReuslt();
            foreach (var d in data)
            {
                subscrption.UserSubscriptorReuslt.Add(new UserSubscriptorReuslt
                {
                    Id = d.Id,
                    Status = ""
                });
            }
            return subscrption;
        }

        public override async Task<ManyUserSubscriptorReuslt> GetAllSubscriptorInSubscription(UserSubscriptor request, ServerCallContext context)
        {
            if (request.Page < 1 || request.SubscriptionId <1)
            {
                throw new BadPageException("Number Page must be positive.");
            }
            var query = new GetAllSubscriptorQuery(request.Page,request.SubscriptionId);
            List<UsersInSubscriptionResponseModel> data = await mediator.Send(query);
            ManyUserSubscriptorReuslt subscrptor = new ManyUserSubscriptorReuslt();
            foreach (var d in data)
            {
                subscrptor.UserSubscriptorReuslt.Add(new UserSubscriptorReuslt
                {
                    Id = d.Id,
                    Status = d.Status,
                    UserId = d.UserId
                });
            }
            return subscrptor;
        }

    }
}
