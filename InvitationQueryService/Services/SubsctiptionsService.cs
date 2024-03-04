using Grpc.Core;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Presentation.Exceptions;
using InvitationQueryTest.QuerySide.GetAllPermissions;
using InvitationQueryTest.QuerySide.GetAllSubscriptionForOwner;
using InvitationQueryTest.QuerySide.GetAllSubscriptionForSubscriptor;
using InvitationQueryTest.QuerySide.GetAllSubscriptor;
using InvitationQueryTest.QuerySide.GetStatus;
using MediatR;

namespace InvitationQueryService.Presentation.Services
{
    public class SubsctiptionsService:Subscriptions.SubscriptionsBase
    {
        private readonly IMediator mediator;

        public SubsctiptionsService(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public override async Task<ManySubscrption> GetAllSubscriptionForOwner(SubscriptionPage request, ServerCallContext context)
        {
            if (request.NumberPage < 1)
            {
                throw new BadPageException("Number Page must be positive.");
            }
            var query = new GetAllSubscriptionForOwnerQuery(request.NumberPage);
            List<SubscriptionsEntity>data = await mediator.Send(query);
            ManySubscrption subscrption = new ManySubscrption();
            foreach (var d in data)
            {
                subscrption.Id.Add(d.Id);
            }
            return subscrption;
        }

        public override async Task<ManySubscrption> GetAllSubscriptionForSubscriptor(SubscriptionPage request, ServerCallContext context)
        {
            if (request.NumberPage < 1)
            {
                throw new BadPageException("Number Page must be positive.");
            }
            var query = new GetAllSubscriptionForSubscriptorQuery(request.NumberPage);
            List<SubscriptionsEntity> data = await mediator.Send(query);
            ManySubscrption subscrption = new ManySubscrption();
            foreach (var d in data)
            {
                subscrption.Id.Add(d.Id);
            }
            return subscrption;
        }
        public override async Task<ManySubscrptor> GetAllSubscriptorInSubscription(SubscriptionPage request, ServerCallContext context)
        {
            if (request.NumberPage < 1)
            {
                throw new BadPageException("Number Page must be positive.");
            }
            var query = new GetAllSubscriptorQuery(request.NumberPage);
            List<SubscriptorEntity> data = await mediator.Send(query);
            ManySubscrptor subscrptor = new ManySubscrptor();
            foreach (var d in data)
            {
                subscrptor.Id.Add(d.Id);
            }
            return subscrptor;
        }

        public override async Task<StatusResult> GetStatusSbuscriptorInSbuscription(Status request, ServerCallContext context)
        {
            if (request.Userid < 1 || request.Ownerid < 1)
            {
                throw new BadPageException("UserId and OwnerId must be positive.");
            }
            var query = new GetStatusQuery(request.Userid , request.Ownerid);
            string data = await mediator.Send(query);
            return new StatusResult
            {
                State = data
            };
        }

    }
}
