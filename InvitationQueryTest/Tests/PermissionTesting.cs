using Grpc.Core;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Infrastructure.Database;
using InvitationQueryTest.DatabaseQuery;
using InvitationQueryTest.Faker;
using InvitationQueryTest.Helper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace InvitationQueryTest.Tests
{
    public class PermissionTesting : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PermissionTesting(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public async Task GetAll_PositivePage_Successfully() {
            Permissions.PermissionsClient _client = new Permissions.PermissionsClient(_factory.CreateGrpcChannel());

            for(int i=0; i<4; i++)
            {
                await DatabaseQueryHelper.AddPermissions(this._factory, $"Permission-{i+1}");
            }

            var data = await _client.GetAllAsync( new GeneratePermissionPage().Generate());
            Assert.NotNull(data);
            Assert.Equal(4, data.Permission.Count());
        }

        [Fact]
        public async Task GetAll_NegativePage_Exception() {
            Permissions.PermissionsClient _client = new Permissions.PermissionsClient(_factory.CreateGrpcChannel());

            PermissionPage permissionPage = new GeneratePermissionPage().Generate();

            permissionPage.NumberPage = -10;
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllAsync(permissionPage);
            });
        }

        [Fact]
        public async Task CheckPermission_Exists_Succesfully()
        {
            Permissions.PermissionsClient _client = new Permissions.PermissionsClient(_factory.CreateGrpcChannel());

            int permissionId = await DatabaseQueryHelper.AddPermissions(this._factory,"Any");
            var res = await _client.CheckAsync(new GeneratePermissionId(permissionId).Generate());
            Assert.NotNull(res);
        }
    }
}
