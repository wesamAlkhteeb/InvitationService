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
            invitationRequest.InvitationInfo = new InvitationInfoRequest()
            {
                AccountId = 1,
                UserId = 1,
                MemberId = 2,
                SubscriptionId = 90
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
            var response = await client.SendInvitationToMemberAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory,"SendEvent", 1);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task SendNewInvitation_MemberWasJoinedAndNeedToRejoinAfterExit_Successfully()
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
            await client.SendInvitationToMemberAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, "SendEvent", 1);

            await client.AcceptAsync(invitationRequest.InvitationInfo);
            DatabaseHelper.CheckEvent(_factory, "AcceptEvent", 2);

            await client.LeaveMemberAsync(invitationRequest.InvitationInfo);
            DatabaseHelper.CheckEvent(_factory, "LeaveEvent", 3);

            var response = await client.JoinMemberByAdminAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, "JoinEvent", 4);

            Assert.NotNull(response);
        }
        
        [Fact]
        public async Task SendNewInvitation_AlreadyExists_Exception()
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
            await client.SendInvitationToMemberAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, "SendEvent", 1);

            await client.AcceptAsync(invitationRequest.InvitationInfo);
            DatabaseHelper.CheckEvent(_factory, "AcceptEvent", 2);

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
            await client.SendInvitationToMemberAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, "SendEvent", 1);

            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.SendInvitationToMemberAsync(invitationRequest);
            });
        }
    }
}
