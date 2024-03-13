using InvitationQueryService.Application.QuerySideServiceBus.Join;
using InvitationQueryService.Application.QuerySideServiceBus.Leave;
using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Models;
using InvitationQueryService.Infrastructure.Database;
using InvitationQueryTest.Helper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace InvitationQueryTest.Tests.ListenerTest
{
    public class LeaveEventTesting:IClassFixture<WebApplicationFactory<Program>>
    {
        public readonly WebApplicationFactory<Program> _factory;
        public LeaveEventTesting(WebApplicationFactory<Program> factory,ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public async Task LeaveInvitationQueryHandler_AddCurrectSequence_Successfully()
        {
            var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var joinQuery = new JoinInvitationQuery
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

            bool isJoinHandle = await mediator.Send(joinQuery);
            Assert.True(isJoinHandle);
            var leaveQuery1 = new LeaveInvitationQuery
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
            bool isLeaveHandle = await mediator.Send(leaveQuery1);
            Assert.True(isLeaveHandle);

            var record = await database.Subscriptors.Where(x => x.SubscriptorAccountId == leaveQuery1.Data.MemberId).FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Out.ToString(), record.Status);
        }

        [Fact]
        public async Task LeaveInvitationQueryHandler_RehundleOldSequence_Successfully()
        {
            var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var joinQuery = new JoinInvitationQuery
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

            bool isJoinHandle = await mediator.Send(joinQuery);
            Assert.True(isJoinHandle);
            var leaveQuery1 = new LeaveInvitationQuery
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
                Sequence = 1
            };
            bool isLeaveHandle = await mediator.Send(leaveQuery1);
            Assert.True(isLeaveHandle);
            bool isLeaveReHandle = await mediator.Send(leaveQuery1);
            Assert.True(isLeaveReHandle);

            var record = await database.Subscriptors.Where(x => x.SubscriptorAccountId == leaveQuery1.Data.MemberId).FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Joined.ToString(), record.Status);
        }

        [Fact]
        public async Task LeaveInvitationQueryHandler_ArriveEventEarly_False()
        {
            var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var joinQuery = new JoinInvitationQuery
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

            bool isJoinHandle = await mediator.Send(joinQuery);
            Assert.True(isJoinHandle);
            
            var leaveQuery1 = new LeaveInvitationQuery
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
                Sequence = 5
            };
            bool isLeaveHandle = await mediator.Send(leaveQuery1);
            Assert.False(isLeaveHandle);

            var record = await database.Subscriptors.Where(x => x.SubscriptorAccountId == leaveQuery1.Data.MemberId).FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Joined.ToString(), record.Status);
        }


    }
}
