﻿using Grpc.Core;
using InvintionCommandTest.Database;
using InvintionCommandTest.Helper;
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
            var response = await client.JoinMemberByAdminAsync(invitationRequest);
            DatabaseHelper.CheckEvent(_factory, "JoinEvent", 1);

            Assert.NotNull(response);
        }
        [Fact]
        public async Task Join_MemberIsAlreadyJoined_Exception()
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
            await client.AcceptAsync(invitationRequest.InvitationInfo);
            DatabaseHelper.CheckEvent(_factory, "AcceptEvent", 2);

            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await client.JoinMemberByAdminAsync(invitationRequest);
            });
        }
    }
}
