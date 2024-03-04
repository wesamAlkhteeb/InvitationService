using Grpc.Core;
using InvintionCommandTest.Helper;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        [Fact]
        public async Task GetAllSubscriptorInSubscription_NegativePage_Exception() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            SubscriptionPage page = new SubscriptionPage
            {
                NumberPage = 0
            };
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptorInSubscriptionAsync(page);
            });
            page.NumberPage = -1;
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptorInSubscriptionAsync(page);
            });
        }
        
        [Fact]
        public async Task GetAllSubscriptorInSubscription_PositivePage_Successfully() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            SubscriptionPage page = new SubscriptionPage
            {
                NumberPage = 1
            };
            var data = await _client.GetAllSubscriptorInSubscriptionAsync(page);
            Assert.NotNull(data);
        }
        
        [Fact]
        public async Task GetAllSubscriptionForSubscriptor_NegativePage_Exception() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            SubscriptionPage page = new SubscriptionPage
            {
                NumberPage = 0
            };
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptionForSubscriptorAsync(page);
            });
            page.NumberPage = -1;
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptionForSubscriptorAsync(page);
            });
        }
        
        [Fact]
        public async Task GetAllSubscriptionForSubscriptor_PositivePage_Successfully() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            SubscriptionPage page = new SubscriptionPage
            {
                NumberPage = 1
            };
            var data = await _client.GetAllSubscriptionForSubscriptorAsync(page);
            Assert.NotNull(data);
        }
        
        [Fact]
        public async Task GetAllSubscriptionForOwner_NegativePage_Exception() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            SubscriptionPage page = new SubscriptionPage
            {
                NumberPage = 0
            };
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptionForOwnerAsync(page);
            });
            page.NumberPage = -1;
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllSubscriptionForOwnerAsync(page);
            });
        }
        [Fact]
        public async Task GetAllSubscriptionForOwner_PositivePage_Successfully() {
            Subscriptions.SubscriptionsClient _client = new Subscriptions.SubscriptionsClient(_factory.CreateGrpcChannel());

            SubscriptionPage page = new SubscriptionPage
            {
                NumberPage = 1
            };
            var data = await _client.GetAllSubscriptionForOwnerAsync(page);
            Assert.NotNull(data);
        }
    }
}
