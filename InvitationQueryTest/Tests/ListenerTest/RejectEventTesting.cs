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
    public class RejectEventTesting:IClassFixture<WebApplicationFactory<Program>>
    {
        public readonly WebApplicationFactory<Program> _factory;
        public RejectEventTesting(WebApplicationFactory<Program> factory,ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public async Task RejectInvitationQueryHandler_AddCurrectSequence_Successfully()
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

            var rejectQuery = new RejectInvitationQuery
            {
                AggregateId = "1-90",
                DateTime = DateTime.UtcNow,
                Id = 1,
                Sequence = 2,
                Data = new InfoModel
                {
                    AccountId = 1,
                    MemberId = 2,
                    SubscriptionId = 1,
                    UserId = 1
                }
            };
            bool isRejectHandle = await mediator.Send(rejectQuery);
            Assert.True(isRejectHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == sendQuery.Data.Info.MemberId)
                .FirstOrDefaultAsync();

            Assert.NotNull(record);
            Assert.Equal(rejectQuery.Data.MemberId, record.SubscriptorAccountId);
            Assert.Equal(InvitationState.Out.ToString(), record.Status);
        }

        [Fact]
        public async Task RejectInvitationQueryHandler_RehundleOldSequence_Successfully()
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

            var rejectQuery = new RejectInvitationQuery
            {
                AggregateId = "1-90",
                DateTime = DateTime.UtcNow,
                Id = 1,
                Sequence = 2,
                Data = new InfoModel
                {
                    AccountId = 1,
                    MemberId = 2,
                    SubscriptionId = 1,
                    UserId = 1
                }
            };
            bool isRejectHandle = await mediator.Send(rejectQuery);
            Assert.True(isRejectHandle);
            bool isRejectReHandle = await mediator.Send(rejectQuery);
            Assert.True(isRejectReHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == sendQuery.Data.Info.MemberId)
                .FirstOrDefaultAsync();

            Assert.NotNull(record);
            Assert.Equal(rejectQuery.Data.MemberId, record.SubscriptorAccountId);
            Assert.Equal(InvitationState.Out.ToString(), record.Status);
        }

        [Fact]
        public async Task RejectInvitationQueryHandler_ArriveEventEarly_False()
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

            var rejectQuery = new RejectInvitationQuery
            {
                AggregateId = "1-90",
                DateTime = DateTime.UtcNow,
                Id = 1,
                Sequence = 10,
                Data = new InfoModel
                {
                    AccountId = 1,
                    MemberId = 2,
                    SubscriptionId = 1,
                    UserId = 1
                }
            };
            bool isRejectHandle = await mediator.Send(rejectQuery);
            Assert.False(isRejectHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == sendQuery.Data.Info.MemberId)
                .FirstOrDefaultAsync();

            Assert.NotNull(record);
            Assert.Equal(rejectQuery.Data.MemberId, record.SubscriptorAccountId);
            Assert.Equal(InvitationState.Pending.ToString(), record.Status);
        }


    }
}
