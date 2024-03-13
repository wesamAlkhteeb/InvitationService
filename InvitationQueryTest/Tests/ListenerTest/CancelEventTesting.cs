using InvitationQueryService.Application.QuerySideServiceBus.Cancel;
using InvitationQueryService.Application.QuerySideServiceBus.Join;
using InvitationQueryService.Application.QuerySideServiceBus.Leave;
using InvitationQueryService.Application.QuerySideServiceBus.Send;
using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Models;
using InvitationQueryService.Infrastructure.Database;
using InvitationQueryTest.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Xunit.Abstractions;

namespace InvitationQueryTest.Tests.ListenerTest
{
    public class CancelEventTesting:IClassFixture<WebApplicationFactory<Program>>
    {
        public readonly WebApplicationFactory<Program> _factory;
        public CancelEventTesting(WebApplicationFactory<Program> factory,ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public async Task CancelInvitationQueryHandler_AddCurrectSequence_Successfully()
        {
            var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var sendQuery = new SendInvitationQuery
            {
                AggregateId = "1-90",
                Data = new DataInfoModel
                {
                    Info = new InfoModel
                    {
                        AccountId = 1,
                        MemberId = 2,
                        SubscriptionId = 1,
                        UserId = 1
                    },
                    Permissions = new List<PermissionModel>
                    {
                        new PermissionModel
                        {
                            Id = 1,
                            Name = "aa"
                        }
                    }
                },
                DateTime = DateTime.UtcNow,
                Id = 1,
                Sequence = 1
            };

            bool isSendHandle = await mediator.Send(sendQuery);
            Assert.True(isSendHandle);
            var cancelQuery1 = new CancelInvitationQuery
            {
                AggregateId = "1-90",
                Data = new InfoModel
                {
                    AccountId = 1,
                    MemberId = 2,
                    SubscriptionId = 1,
                    UserId = 1
                },
                dateTime = DateTime.UtcNow,
                Id = 1,
                Sequence = 2
            };
            bool isLeaveHandle = await mediator.Send(cancelQuery1);
            Assert.True(isLeaveHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == cancelQuery1.Data.MemberId)
                .FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Out.ToString(), record.Status);
        }

        [Fact]
        public async Task CancelInvitationQueryHandler_RehundleOldSequence_Successfully()
        {
            var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var sendQuery = new SendInvitationQuery
            {
                AggregateId = "1-90",
                Data = new DataInfoModel
                {
                    Info = new InfoModel
                    {
                        AccountId = 1,
                        MemberId = 2,
                        SubscriptionId = 1,
                        UserId = 1
                    },
                    Permissions = new List<PermissionModel>
                    {
                        new PermissionModel
                        {
                            Id = 1,
                            Name = "aa"
                        }
                    }
                },
                DateTime = DateTime.UtcNow,
                Id = 1,
                Sequence = 1
            };

            bool isSendHandle = await mediator.Send(sendQuery);
            Assert.True(isSendHandle);
            var cancelQuery1 = new CancelInvitationQuery
            {
                AggregateId = "1-90",
                Data = new InfoModel
                {
                    AccountId = 1,
                    MemberId = 2,
                    SubscriptionId = 1,
                    UserId = 1
                },
                dateTime = DateTime.UtcNow,
                Id = 1,
                Sequence = 2
            };
            bool isCancelHandle = await mediator.Send(cancelQuery1);
            Assert.True(isCancelHandle);
            bool isCancelReHandle = await mediator.Send(cancelQuery1);
            Assert.True(isCancelHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == cancelQuery1.Data.MemberId)
                .FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Out.ToString(), record.Status);
        }

        [Fact]
        public async Task CancelInvitationQueryHandler_ArriveEventEarly_False()
        {
            var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var sendQuery = new SendInvitationQuery
            {
                AggregateId = "1-90",
                Data = new DataInfoModel
                {
                    Info = new InfoModel
                    {
                        AccountId = 1,
                        MemberId = 2,
                        SubscriptionId = 1,
                        UserId = 1
                    },
                    Permissions = new List<PermissionModel>
                    {
                        new PermissionModel
                        {
                            Id = 1,
                            Name = "aa"
                        }
                    }
                },
                DateTime = DateTime.UtcNow,
                Id = 1,
                Sequence = 1
            };

            bool isSendHandle = await mediator.Send(sendQuery);
            Assert.True(isSendHandle);
            var cancelQuery1 = new CancelInvitationQuery
            {
                AggregateId = "1-90",
                Data = new InfoModel
                {
                    AccountId = 1,
                    MemberId = 2,
                    SubscriptionId = 1,
                    UserId = 1
                },
                dateTime = DateTime.UtcNow,
                Id = 1,
                Sequence = 10
            };
            bool isCancelHandle = await mediator.Send(cancelQuery1);
            Assert.False(isCancelHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == cancelQuery1.Data.MemberId)
                .FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Pending.ToString(), record.Status);

        }


    }
}
