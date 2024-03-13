using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Models;
using InvitationQueryService.Infrastructure.Database;
using InvitationQueryService.Models.QuerySideServiceBus;
using InvitationQueryTest.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Xunit.Abstractions;

namespace InvitationQueryTest.Tests.ListenerTest
{
    public class AcceptEventTesting:IClassFixture<WebApplicationFactory<Program>>
    {
        public readonly WebApplicationFactory<Program> _factory;
        public AcceptEventTesting(WebApplicationFactory<Program> factory,ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public async Task AcceptInvitationQueryHandler_AddCurrectSequence_Successfully()
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


            var acceptQuery1 = new AcceptInvitationQuery
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
            bool isCancelHandle = await mediator.Send(acceptQuery1);
            Assert.True(isCancelHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == acceptQuery1.Data.MemberId)
                .FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Joined.ToString(), record.Status);
        }

        [Fact]
        public async Task AcceptInvitationQueryHandler_RehundleOldSequence_Successfully()
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
            
            
            var acceptQuery1 = new AcceptInvitationQuery
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
            bool isCancelHandle = await mediator.Send(acceptQuery1);
            Assert.True(isCancelHandle);
            bool isCancelReHandle = await mediator.Send(acceptQuery1);
            Assert.True(isCancelHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == acceptQuery1.Data.MemberId)
                .FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Joined.ToString(), record.Status);
        }

        [Fact]
        public async Task AcceptInvitationQueryHandler_ArriveEventEarly_False()
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


            var acceptQuery1 = new AcceptInvitationQuery
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
                Sequence = 20
            };
            bool isCancelHandle = await mediator.Send(acceptQuery1);
            Assert.False(isCancelHandle);
            
            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == acceptQuery1.Data.MemberId)
                .FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Pending.ToString(), record.Status);
        }
    }
}
