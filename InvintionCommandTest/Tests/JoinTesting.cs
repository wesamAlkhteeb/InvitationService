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
    public class JoinTesting : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public JoinTesting(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
                services.RejectServiceBus();
            });
        }

        [Fact]
        public async Task Join_MemberIsNotJoined_Successfully()
        {
            Invitation.InvitationClient client = new Invitation.InvitationClient(_factory.CreateGrpcChannel());

            InvitationRequest invitationRequest = new InvitationRequest();
            invitationRequest.InvitationInfo = new GenerateInvitationInfoRequest().Generate();
            invitationRequest.Permissions.Add(new GeneratePermission(1).Generate());
            invitationRequest.Permissions.Add(new GeneratePermission(2).Generate());

            var response = await client.JoinMemberByAdminAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, EventType.JoinEvent.ToString(), 1);

            Assert.NotNull(response);
        }
        [Fact]
        public async Task Join_MemberIsAlreadyJoined_Exception()
        {
            Invitation.InvitationClient client = new Invitation.InvitationClient(_factory.CreateGrpcChannel());

            InvitationRequest invitationRequest = new InvitationRequest();
            invitationRequest.InvitationInfo = new GenerateInvitationInfoRequest().Generate();
            invitationRequest.Permissions.Add(new GeneratePermission(1).Generate());
            invitationRequest.Permissions.Add(new GeneratePermission(2).Generate());
            await client.SendInvitationToMemberAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, EventType.SendEvent.ToString(), 1);
            await client.AcceptAsync(invitationRequest.InvitationInfo);
            DatabaseHelper.CheckEvent(_factory, EventType.AcceptEvent.ToString(), 2);

            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.JoinMemberByAdminAsync(invitationRequest);
            });
        }
    }
}
