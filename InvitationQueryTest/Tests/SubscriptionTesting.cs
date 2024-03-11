using Grpc.Core;
using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Infrastructure.Database;
using InvitationQueryTest.Helper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace InvitationQueryTest.Tests
{
    public class SubscriptionTesting:IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public SubscriptionTesting(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
            //DatabaseHelper.AddSubscriptionAndSubscriptor(factory);
        }

        [Fact]
        public async Task GetAllSubscriptorInSubscription_NegativePage_Exception() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            UserSubscriptor page = new UserSubscriptor
            {
                Page = 0,
                SubscriptionId = 1
            };
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptorInSubscriptionAsync(page);
            });
            page.Page = -1;
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptorInSubscriptionAsync(page);
            });
        }
        
        [Fact]
        public async Task GetAllSubscriptorInSubscription_PositivePage_Successfully() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            var scope = this._factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            database.Subscriptions.Add(new SubscriptionsEntity
            {
                AccountId = 400,
                Type = SubscriptionType.A.ToString()
            });
            for(int i=0; i<4; i++)
            {
                database.Subscriptors.Add(new SubscriptorEntity
                {
                    Sequence = 1,
                    Status = InvitationState.Joined.ToString(),
                    SubscriptionId = 2,
                    SubscriptorAccountId = i+1
                });
            }
            database.SaveChanges();
            UserSubscriptor page = new UserSubscriptor
            {
                Page = 1,
                SubscriptionId = 2
            };
            var data = await _client.GetAllSubscriptorInSubscriptionAsync(page);
            Assert.NotNull(data);
            Assert.Equal(4, data.UserSubscriptionReuslt.Count());
        }
        
        [Fact]
        public async Task GetAllSubscriptionForSubscriptor_NegativePage_Exception() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            UserSubscription page = new UserSubscription
            {
                Page = 0,
                UserId = 1
            };
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptionForSubscriptorAsync(page);
            });
            page.Page = -1;
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptionForSubscriptorAsync(page);
            });
        }
        
        [Fact]
        public async Task GetAllSubscriptionForSubscriptor_PositivePage_Successfully() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            UserSubscription page = new UserSubscription
            {
                Page = 1,
                UserId = 200
            };

            var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            for (int i = 0; i < 4; i++)
            {
                var subscription = new SubscriptionsEntity
                {
                    AccountId = 300,
                    Type = SubscriptionType.A.ToString()
                };
                database.Subscriptions.Add(subscription);
            }
            for(int i=1; i<=5; i++)
            {
                database.Subscriptors.Add(new SubscriptorEntity
                {
                    Sequence = i,
                    Status = InvitationState.Joined.ToString(),
                    SubscriptionId = i,
                    SubscriptorAccountId = 200
                });
            }
            database.SaveChanges();
            var data = await _client.GetAllSubscriptionForSubscriptorAsync(page);
            
            Assert.NotNull(data);
            Assert.Equal(5, data.UserSubscriptionReuslt.Count());
        }
        
        [Fact]
        public async Task GetAllSubscriptionForOwner_NegativePage_Exception() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            OwnerSubscription page = new OwnerSubscription
            {
                Page = 0,
                OwnerId = 10
            };
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptionForOwnerAsync(page);
            });
            page.Page = -1;
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptionForOwnerAsync(page);
            });
        }
        [Fact]
        public async Task GetAllSubscriptionForOwner_PositivePage_Successfully() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());
            OwnerSubscription page = new OwnerSubscription
            {
                Page = 1,
                OwnerId = 300
            };

            var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            for(int i=0; i<4; i++)
            {
                database.Subscriptions.Add(new SubscriptionsEntity
                {
                    AccountId = 300,
                    Type = SubscriptionType.A.ToString()
                });
            }
            database.SaveChanges();

            var data = await _client.GetAllSubscriptionForOwnerAsync(page);
            Assert.NotNull(data);
            Assert.Equal(4, data.OwnerSubscriptionReuslt.Count());
        }
    }
}
