using InvitationQueryService.Application.QuerySideServiceBus.Join;
using InvitationQueryService.Application.QuerySideServiceBus.Leave;
using InvitationQueryService.Application.QuerySideServiceBus.Remove;
using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Models;
using InvitationQueryService.Infrastructure.Database;
using InvitationQueryService.Models.QuerySideServiceBus;
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
    public class RemoveEventTesting : IClassFixture<WebApplicationFactory<Program>>
    {
        public readonly WebApplicationFactory<Program> _factory;
        public RemoveEventTesting(WebApplicationFactory<Program> factory,ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public async Task RemoveInvitationQueryHandler_AddCurrectSequence_Successfully()
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
            var removeQuery1 = new RemoveInvitationQuery
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
            bool isRemoveHandle = await mediator.Send(removeQuery1);
            Assert.True(isRemoveHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == removeQuery1.Data.MemberId)
                .FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Out.ToString(), record.Status);
        }

        [Fact]
        public async Task RemoveInvitationQueryHandler_RehundleOldSequence_Successfully()
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
            var removeQuery1 = new RemoveInvitationQuery
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
            bool isRemoveHandle = await mediator.Send(removeQuery1);
            Assert.True(isRemoveHandle);
            bool isRemoveReHandle = await mediator.Send(removeQuery1);
            Assert.True(isRemoveReHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == removeQuery1.Data.MemberId)
                .FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Out.ToString(), record.Status);
        }

        [Fact]
        public async Task RemoveInvitationQueryHandler_ArriveEventEarly_False()
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
            var removeQuery1 = new RemoveInvitationQuery
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
            bool isRemoveHandle = await mediator.Send(removeQuery1);
            Assert.True(isRemoveHandle);

            removeQuery1.Sequence = 10;
            bool isRemoveReHandle = await mediator.Send(removeQuery1);
            Assert.False(isRemoveReHandle);

            var record = await database.Subscriptors
                .Where(x => x.SubscriptorAccountId == removeQuery1.Data.MemberId)
                .FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Out.ToString(), record.Status);
        }


    }
}
