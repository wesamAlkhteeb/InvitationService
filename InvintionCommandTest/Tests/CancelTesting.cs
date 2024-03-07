using Grpc.Core;
using InvintionCommandTest.Database;
using InvintionCommandTest.Helper;
using InvitationCommandTest;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace InvintionCommandTest.Tests
{
    public class CancelTesting : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CancelTesting(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
                services.RejectServiceBus();
            });
        }

        [Fact]
        public async Task CancelInvitation_MemberIsFoundInSubscriptionButNeedToCancel_Successfully()
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
            await client.SendInvitationToMemberAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, "SendEvent", 1);
            var response = await client.CancelAsync(invitationRequest.InvitationInfo);
            DatabaseHelper.CheckEvent(_factory, "CancelEvent", 2);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CancelInvitation_MemberIsNotFoundInSubscription_Exception()
        {
            Invitation.InvitationClient client = new Invitation.InvitationClient(_factory.CreateGrpcChannel());

            InvitationInfoRequest invitationInfo = new InvitationInfoRequest()
            {
                AccountId = 2,
                UserId = 2,
                MemberId = 3,
                SubscriptionId = 91
            };
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.CancelAsync(invitationInfo);
            });
        }
    }
}
