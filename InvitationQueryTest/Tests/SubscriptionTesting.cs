using Grpc.Core;
using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Infrastructure.Database;
using InvitationQueryTest.DatabaseQuery;
using InvitationQueryTest.Faker;
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

            int memberId = 200 , accountId=400;
            int subscriptionId = await DatabaseQueryHelper.AddSubscription(this._factory,accountId);            
            for (int i = 0; i < 4; i++)
            {
                await DatabaseQueryHelper.AddSubscriptor(this._factory,i+1,subscriptionId,memberId);
            }
            UserSubscriptor page = new GenerateUserSubscriptor(subscriptionId).Generate();
            var data = await _client.GetAllSubscriptorInSubscriptionAsync(page);
            Assert.NotNull(data);
            Assert.Equal(4, data.UserSubscriptionReuslt.Count());
        }
        
        [Fact]
        public async Task GetAllSubscriptionForSubscriptor_NegativePage_Exception() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            int memberId = 2;
            UserSubscription page = new GenerateUserSubscription(memberId).Generate();

            page.Page = -1;
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptionForSubscriptorAsync(page);
            });
        }
        
        [Fact]
        public async Task GetAllSubscriptionForSubscriptor_PositivePage_Successfully() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());
            
            int accountId = 3, memberId = 2;
            for(int i=0; i<4; i++)
            {
                int subscriptionId = await DatabaseQueryHelper.AddSubscription(this._factory, accountId);
                await DatabaseQueryHelper.AddSubscriptor(this._factory, i + 1, subscriptionId, memberId);
            }

            UserSubscription page = new GenerateUserSubscription(memberId).Generate();
            var data = await _client.GetAllSubscriptionForSubscriptorAsync(page);
            
            Assert.NotNull(data);
            Assert.Equal(4, data.UserSubscriptionReuslt.Count());
        }
        
        [Fact]
        public async Task GetAllSubscriptionForOwner_NegativePage_Exception() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            int ownerId = 10;
            OwnerSubscription page = new GenerateOwnerSubscription(ownerId);
            
            page.Page = -1;
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptionForOwnerAsync(page);
            });
        }
        [Fact]
        public async Task GetAllSubscriptionForOwner_PositivePage_Successfully() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            int ownerId = 300;
            OwnerSubscription page = new GenerateOwnerSubscription(ownerId);

            for (int i = 0; i < 4; i++)
            {
                await DatabaseQueryHelper.AddSubscription(this._factory, ownerId);
            }

            var data = await _client.GetAllSubscriptionForOwnerAsync(page);
            Assert.NotNull(data);
            Assert.Equal(4, data.OwnerSubscriptionReuslt.Count());
        }
    }
}
