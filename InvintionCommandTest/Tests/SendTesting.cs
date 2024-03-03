using Google.Protobuf.Collections;
using Grpc.Core;
using InvintionCommandTest.Helper;
using InvitationCommandTest;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace InvintionCommandTest.Tests
{
    public class SendTesting:IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public SendTesting(WebApplicationFactory<Program> factory,ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact] 
        public async Task SendNewInvitation_FirstTime_Successfully(){
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
            
            Assert.NotNull(response);
        } 


        // Joined  => exited => send 
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
            await client.AcceptAsync(invitationRequest.InvitationInfo);
            await client.LeaveMemberAsync(invitationRequest.InvitationInfo);
            
            var response = await client.JoinMemberByAdminAsync(invitationRequest);
            Assert.NotNull(response);
        } 
        [Fact] 
        public async Task SendNewInvitation_AlreadyExists_Exception(){
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
            await client.AcceptAsync(invitationRequest.InvitationInfo);
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.SendInvitationToMemberAsync(invitationRequest);
            });
        }
        
        // send invite and then need some time to response memper (accept | reject)
        [Fact] 
        public async Task SendNewInvitation_Pinding_Exception(){
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
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.SendInvitationToMemberAsync(invitationRequest);
            });
        }
    }
}
