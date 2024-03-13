using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Entities;
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
    public class SendEventTesting:IClassFixture<WebApplicationFactory<Program>>
    {
        public readonly WebApplicationFactory<Program> _factory;
        public SendEventTesting(WebApplicationFactory<Program> factory,ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public async Task SendInvitationQueryHandler_AddCurrectSequence_Successfully()
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

            var record = await database.Subscriptors.Where(x => x.SubscriptorAccountId == sendQuery.Data.Info.MemberId).FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Pending.ToString(), record.Status);

            var permissionRecords = await database.SubscriptionPermissions
                .Where(x => x.SubscriptorId == record.Id)
                .OrderBy(x => x.PermissionId).ToListAsync();
            int count = 0;
            foreach (var permission in permissionRecords)
            {
                PermissionModel? sendPermission = sendQuery.Data.Permissions.Find(x => x.Id == permission.Id);
                if (sendPermission != null)
                {
                    count++;
                    Assert.Equal(permission.Id, sendPermission.Id);
                }
            }
            Assert.Equal(sendQuery.Data.Permissions.Count(), count);
        }

        [Fact]
        public async Task SendInvitationQueryHandler_RehundleOldSequence_Successfully()
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
            bool isSendHandle1 = await mediator.Send(sendQuery);
            Assert.True(isSendHandle1);

            var record = await database.Subscriptors.Where(x => x.SubscriptorAccountId == sendQuery.Data.Info.MemberId).FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Pending.ToString(), record.Status);
        }

        [Fact]
        public async Task SendInvitationQueryHandler_ArriveEventEarly_False()
        {
            var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            database.Subscriptors.Add(new SubscriptorEntity
            {
                Sequence = 1,
                Status = InvitationState.Out.ToString(),
                SubscriptionId = 1,
                SubscriptorAccountId = 10
            });
            await database.SaveChangesAsync();

            var sendQuery = new SendInvitationQuery
            {
                AggregateId = "1-90",
                Data = new DataInfoModel
                {
                    Info = new InfoModel
                    {
                        AccountId = 1,
                        MemberId = 10,
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
                Sequence = 10
            };

            bool isSendHandle = await mediator.Send(sendQuery);
            Assert.False(isSendHandle);
            
            var record = await database.Subscriptors.Where(x => x.SubscriptorAccountId == sendQuery.Data.Info.MemberId).FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Out.ToString(), record.Status);
        }


    }
}
