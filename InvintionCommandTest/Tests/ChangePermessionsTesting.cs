using Grpc.Core;
using InvintionCommandTest.Database;
using InvintionCommandTest.Helper;
using InvitationCommandTest;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace InvintionCommandTest.Tests
{
    public class ChangePermessionsTesting : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ChangePermessionsTesting(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
                services.RejectServiceBus();
            });
        }

        [Fact]
        public async Task ChangePermessions_MemberIsFoundInSubscriptionAndDeleteOnePermission_Successfully()
        {
            Invitation.InvitationClient client = new Invitation.InvitationClient(_factory.CreateGrpcChannel());

            InvitationRequest invitationRequest = new InvitationRequest();
            invitationRequest.InvitationInfo = new InvitationInfoRequest()
            {
                AccountId = 2,
                UserId = 2,
                MemberId = 3,
                SubscriptionId = 1
            };
            invitationRequest.Permissions.Add(new Permissions
            {
                Id = 1,
                Name = "Transfer"
            });
            invitationRequest.Permissions.Add(new Permissions
            {
                Id = 2,
                Name = "PurchaseCards"
            });
            await client.JoinMemberByAdminAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, "JoinEvent", 1);
            invitationRequest.Permissions.RemoveAt(0); 
            
            var response = await client.ChangePermissionsAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, "ChangePermissionEvent", 2);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task ChangePermessions_MemberIsFoundInSubscriptionAndChangeAllPermission_Successfully()
        {
            Invitation.InvitationClient client = new Invitation.InvitationClient(_factory.CreateGrpcChannel());

            InvitationRequest invitationRequest = new InvitationRequest();
            invitationRequest.InvitationInfo = new InvitationInfoRequest()
            {
                AccountId = 2,
                UserId = 2,
                MemberId = 3,
                SubscriptionId = 1
            };
            invitationRequest.Permissions.Add(new Permissions
            {
                Id = 1,
                Name = "Transfer"
            });
            invitationRequest.Permissions.Add(new Permissions
            {
                Id = 2,
                Name = "PurchaseCards"
            });
            await client.JoinMemberByAdminAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, "JoinEvent", 1);
            invitationRequest.Permissions[0].Id = 4;
            invitationRequest.Permissions[0].Id = 5;
            var response = await client.ChangePermissionsAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, "ChangePermissionEvent", 2);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task ChangePermessions_MemberIsFoundInSubscriptionBuSetSamePermissions_Successfully()
        {
            Invitation.InvitationClient client = new Invitation.InvitationClient(_factory.CreateGrpcChannel());

            InvitationRequest invitationRequest = new InvitationRequest();
            invitationRequest.InvitationInfo = new InvitationInfoRequest()
            {
                AccountId = 2,
                UserId = 2,
                MemberId = 3,
                SubscriptionId = 1
            };
            invitationRequest.Permissions.Add(new Permissions
            {
                Id = 1,
                Name = "Transfer"
            });
            invitationRequest.Permissions.Add(new Permissions
            {
                Id = 2,
                Name = "PurchaseCards"
            });
            await client.JoinMemberByAdminAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, "JoinEvent", 1);

            var exception = await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.ChangePermissionsAsync(invitationRequest);
            });
        }


        [Fact]
        public async Task ChangePermessions_MemberIsNotFoundInSubscription_Exception()
        {
            Invitation.InvitationClient client = new Invitation.InvitationClient(_factory.CreateGrpcChannel());

            InvitationRequest invitationRequest = new InvitationRequest();
            invitationRequest.InvitationInfo = new InvitationInfoRequest()
            {
                AccountId = 2,
                UserId = 2,
                MemberId = 3,
                SubscriptionId = 91
            };
            invitationRequest.Permissions.Add(new Permissions
            {
                Id = 1,
                Name = "Transfer"
            });
            invitationRequest.Permissions.Add(new Permissions
            {
                Id = 2,
                Name = "PurchaseCards"
            });
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.ChangePermissionsAsync(invitationRequest);
            });
        }
    }
}
