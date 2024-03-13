using Grpc.Core;
using InvintionCommandTest.Database;
using InvintionCommandTest.Faker;
using InvintionCommandTest.Helper;
using InvitationCommandService.Domain;
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
            invitationRequest.InvitationInfo = new GenerateInvitationInfoRequest().Generate();
            invitationRequest.Permissions.Add(new GeneratePermission(1).Generate());
            invitationRequest.Permissions.Add(new GeneratePermission(2).Generate());

            await client.SendInvitationToMemberAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, EventType.SendEvent.ToString(), 1);
            var response = await client.CancelAsync(invitationRequest.InvitationInfo);
            DatabaseHelper.CheckEvent(_factory, EventType.CancelEvent.ToString(), 2);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CancelInvitation_MemberIsNotFoundInSubscription_Exception()
        {
            Invitation.InvitationClient client = new Invitation.InvitationClient(_factory.CreateGrpcChannel());

            InvitationInfoRequest invitationInfo = new GenerateInvitationInfoRequest().Generate();

            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.CancelAsync(invitationInfo);
            });
        }
    }
}
