using Grpc.Core;
using InvintionCommandTest.Database;
using InvintionCommandTest.Helper;
using InvitationCommandTest;
using InvitationCommandService.Database;
using InvitationCommandService.Domain.Entities.Events;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using InvintionCommandTest.Faker;
using InvitationCommandService.Domain;

namespace InvintionCommandTest.Tests
{
    public class SendTesting : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        private readonly InvitationDbContext database;

        public SendTesting(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
                services.RejectServiceBus();
            });
        }

        [Fact]
        public async Task SendNewInvitation_FirstTime_Successfully()
        {
            Invitation.InvitationClient client = new Invitation.InvitationClient(_factory.CreateGrpcChannel());

            InvitationRequest invitationRequest = new InvitationRequest();
            invitationRequest.InvitationInfo = new GenerateInvitationInfoRequest().Generate();
            invitationRequest.Permissions.Add(new GeneratePermission(1).Generate());
            invitationRequest.Permissions.Add(new GeneratePermission(2).Generate());

            var response = await client.SendInvitationToMemberAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory,EventType.SendEvent.ToString(), 1);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task SendNewInvitation_MemberWasJoinedAndNeedToRejoinAfterExit_Successfully()
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

            await client.LeaveMemberAsync(invitationRequest.InvitationInfo);
            DatabaseHelper.CheckEvent(_factory, EventType.LeaveEvent.ToString(), 3);

            var response = await client.SendInvitationToMemberAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, EventType.SendEvent.ToString(), 4);

            Assert.NotNull(response);
        }
        
        [Fact]
        public async Task SendNewInvitation_AlreadyExists_Exception()
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
                await client.SendInvitationToMemberAsync(invitationRequest);
            });
        }

        // send invite and then need some time to response memper (accept | reject)
        [Fact]
        public async Task SendNewInvitation_Pending_Exception()
        {
            Invitation.InvitationClient client = new Invitation.InvitationClient(_factory.CreateGrpcChannel());

            InvitationRequest invitationRequest = new InvitationRequest();
            invitationRequest.InvitationInfo = new GenerateInvitationInfoRequest().Generate();
            invitationRequest.Permissions.Add(new GeneratePermission(1).Generate());
            invitationRequest.Permissions.Add(new GeneratePermission(2).Generate());

            await client.SendInvitationToMemberAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, EventType.SendEvent.ToString(), 1);

            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.SendInvitationToMemberAsync(invitationRequest);
            });
        }
    }
}
