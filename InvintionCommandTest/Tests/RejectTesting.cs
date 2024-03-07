﻿using Grpc.Core;
using InvintionCommandTest.Database;
using InvintionCommandTest.Helper;
using InvitationCommandTest;
using Microsoft.AspNetCore.Mvc.Testing;

using Xunit.Abstractions;

namespace InvintionCommandTest.Tests
{
    public class RejectTesting : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public RejectTesting(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
                services.RejectServiceBus();
            });
        }

        [Fact]
        public async Task RejectInvitation_InvitationHasSended_Successfully()
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
            var response = await client.RejectAsync(invitationRequest.InvitationInfo);
            DatabaseHelper.CheckEvent(_factory, "RejectEvent", 2);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task RejectInvitation_NotfoundInvitation_Exception()
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
                await client.RejectAsync(invitationInfo);
            });
        }

        [Fact]
        public async Task RejectInvitation_MemberIsAccepted_Exception()
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
                await client.RejectAsync(invitationRequest.InvitationInfo);
            });
        }
    }
}
