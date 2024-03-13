using Grpc.Core;
using InvintionCommandTest.Database;
using InvintionCommandTest.Helper;

using InvitationCommandTest;
using InvitationCommandService.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using InvintionCommandTest.Faker;
using InvitationCommandService.Domain;

namespace InvintionCommandTest.Tests
{
    public class RemoveTesting : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly InvitationDbContext database;

        public RemoveTesting(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
                services.RejectServiceBus();
            });
            var scope = factory.Services.CreateScope();
            this.database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
        }

        [Fact]
        public async Task Remove_MemberIsAlreadyJoined_Successfully()
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
            var response = await client.RemoveMemberAsync(invitationRequest.InvitationInfo);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Remove_MemberIsNotJoined_Exception()
        {
            Invitation.InvitationClient client = new Invitation.InvitationClient(_factory.CreateGrpcChannel());
            InvitationInfoRequest invitationInfo = new GenerateInvitationInfoRequest().Generate();
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.RemoveMemberAsync(invitationInfo);
            });
        }
    }
}
