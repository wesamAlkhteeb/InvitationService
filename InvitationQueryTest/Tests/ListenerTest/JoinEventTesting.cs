using InvitationQueryService.Application.QuerySideServiceBus.Join;
using InvitationQueryService.Application.QuerySideServiceBus.Leave;
using InvitationQueryService.Application.QuerySideServiceBus.Send;
using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Entities;
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
    public class JoinEventTesting:IClassFixture<WebApplicationFactory<Program>>
    {
        public readonly WebApplicationFactory<Program> _factory;
        public JoinEventTesting(WebApplicationFactory<Program> factory,ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public async Task JoinInvitationQueryHandler_AddCurrectSequence_Successfully()
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

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == joinQuery.Data.Info.MemberId)
                .FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Joined.ToString(), record.Status);

            var permissionRecords = await database.SubscriptionPermissions
                .Where(x => x.SubscriptorId == record.Id)
                .OrderBy(x=>x.PermissionId).ToListAsync();
            int count = 0;
            foreach (var permission in permissionRecords)
            {
                PermissionModel? joinPermission = joinQuery.Data.Permissions.Find(x => x.Id == permission.Id);
                if(joinPermission != null)
                {
                    count++;
                    Assert.Equal(permission.Id, joinPermission.Id);
                }
            }
            Assert.Equal(joinQuery.Data.Permissions.Count(), count);
        }

        [Fact]
        public async Task JoinInvitationQueryHandler_RehundleOldSequence_Successfully()
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

            bool isJoinReHandle = await mediator.Send(joinQuery);
            Assert.True(isJoinReHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == joinQuery.Data.Info.MemberId)
                .FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Joined.ToString(), record.Status);
        }

        [Fact]
        public async Task JoinInvitationQueryHandler_ArriveEventEarly_False()
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

            joinQuery.Sequence = 100;
            bool isJoinReHandle = await mediator.Send(joinQuery);
            Assert.False(isJoinReHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == joinQuery.Data.Info.MemberId)
                .FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Joined.ToString(), record.Status);
        }


    }
}
