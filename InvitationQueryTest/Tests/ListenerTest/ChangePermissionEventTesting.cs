using InvitationQueryService.Application.QuerySideServiceBus.ChangePermission;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace InvitationQueryTest.Tests.ListenerTest
{
    public class ChangePermissionEventTesting:IClassFixture<WebApplicationFactory<Program>>
    {
        public readonly WebApplicationFactory<Program> _factory;
        public ChangePermissionEventTesting(WebApplicationFactory<Program> factory,ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public async Task ChangePermissionEventTesting_AddCurrectSequence_Successfully()
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

            var record = await database.Subscriptors.Where(x => x.SubscriptorAccountId == joinQuery.Data.Info.MemberId).FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Joined.ToString(), record.Status);

            var permissionRecords = await database.SubscriptionPermissions
                .Where(x => x.SubscriptorId == record.Id)
                .OrderBy(x => x.PermissionId).ToListAsync();
            int count = 0;
            foreach (var permission in permissionRecords)
            {
                PermissionModel? sendPermission = joinQuery.Data.Permissions.Find(x => x.Id == permission.Id);
                if (sendPermission != null)
                {
                    count++;
                    Assert.Equal(permission.Id, sendPermission.Id);
                }
            }
            Assert.Equal(joinQuery.Data.Permissions.Count(), count);

            // change permission
            var changePermissionsQuery = new ChangePermissionsInvitationQuery
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
                            Id = 2,
                            Name = "aa"
                        }
                    }
                },
                DateTime = DateTime.UtcNow,
                Id = 1,
                Sequence = 2
            };
            bool isChangePermissionsHandle = await mediator.Send(changePermissionsQuery);
            Assert.True(isChangePermissionsHandle);

            var permissionRecordsAfter = await database.SubscriptionPermissions
                .Where(x => x.SubscriptorId == record.Id)
                .OrderBy(x => x.PermissionId).ToListAsync();
            int countAfterChange = 0;
            foreach (var permission in permissionRecordsAfter)
            {
                PermissionModel? changePermission = changePermissionsQuery!.Data.Permissions.Find(x => x.Id == permission.Id);
                if (changePermission != null)
                {
                    countAfterChange++;
                    Assert.Equal(permission.Id, changePermission.Id);
                }
            }
            Assert.Equal(changePermissionsQuery!.Data.Permissions.Count(), countAfterChange);
        }

        [Fact]
        public async Task ChangePermissionEventTesting_RehundleOldSequence_Successfully()
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

            var record = await database.Subscriptors.Where(x => x.SubscriptorAccountId == joinQuery.Data.Info.MemberId).FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Joined.ToString(), record.Status);

            var permissionRecords = await database.SubscriptionPermissions
                .Where(x => x.SubscriptorId == record.Id)
                .OrderBy(x => x.PermissionId).ToListAsync();
            int count = 0;
            foreach (var permission in permissionRecords)
            {
                PermissionModel? sendPermission = joinQuery.Data.Permissions.Find(x => x.Id == permission.Id);
                if (sendPermission != null)
                {
                    count++;
                    Assert.Equal(permission.Id, sendPermission.Id);
                }
            }
            Assert.Equal(joinQuery.Data.Permissions.Count(), count);

            // change permission
            var changePermissionsQuery = new ChangePermissionsInvitationQuery
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
                            Id = 2,
                            Name = "aa"
                        }
                    }
                },
                DateTime = DateTime.UtcNow,
                Id = 1,
                Sequence = 2
            };
            bool isChangePermissionsHandle = await mediator.Send(changePermissionsQuery);
            Assert.True(isChangePermissionsHandle);
            bool isChangePermissionsReHandle = await mediator.Send(changePermissionsQuery);
            Assert.True(isChangePermissionsReHandle);

            var permissionRecordsAfter = await database.SubscriptionPermissions
                .Where(x => x.SubscriptorId == record.Id)
                .OrderBy(x => x.PermissionId).ToListAsync();
            int countAfterChange = 0;
            foreach (var permission in permissionRecordsAfter)
            {
                PermissionModel? changePermission = changePermissionsQuery!.Data.Permissions.Find(x => x.Id == permission.Id);
                if (changePermission != null)
                {
                    countAfterChange++;
                    Assert.Equal(permission.Id, changePermission.Id);
                }
            }
            Assert.Equal(changePermissionsQuery!.Data.Permissions.Count(), countAfterChange);
        }

        [Fact]
        public async Task ChangePermissionEventTesting_ArriveEventEarly_False()
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

            var record = await database.Subscriptors.Where(x => x.SubscriptorAccountId == joinQuery.Data.Info.MemberId).FirstOrDefaultAsync();
            Assert.NotNull(record);
            Assert.Equal(InvitationState.Joined.ToString(), record.Status);

            
            // change permission
            var changePermissionsQuery = new ChangePermissionsInvitationQuery
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
                            Id = 2,
                            Name = "aa"
                        }
                    }
                },
                DateTime = DateTime.UtcNow,
                Id = 1,
                Sequence = 10
            };
            bool isChangePermissionsHandle = await mediator.Send(changePermissionsQuery);
            Assert.False(isChangePermissionsHandle);


            var permissionRecords = await database.SubscriptionPermissions
                .Where(x => x.SubscriptorId == record.Id)
                .OrderBy(x => x.PermissionId).ToListAsync();
            int count = 0;
            foreach (var permission in permissionRecords)
            {
                PermissionModel? sendPermission = joinQuery.Data.Permissions.Find(x => x.Id == permission.Id);
                if (sendPermission != null)
                {
                    count++;
                    Assert.Equal(permission.Id, sendPermission.Id);
                }
            }
            Assert.Equal(joinQuery.Data.Permissions.Count(), count);

        }


    }
}
