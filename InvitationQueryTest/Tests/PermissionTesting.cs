using Grpc.Core;
using InvintionCommandTest.Helper;
using InvitationQueryService;
using Microsoft.AspNetCore.Mvc.Testing;
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

            var data = await _client.GetAllAsync(new PermissionPage
            {
                NumberPage = 1
            });
            Assert.NotNull(data);
        }

        [Fact]
        public async Task GetAll_NegativePage_Exception() {
            Permissions.PermissionsClient _client = new Permissions.PermissionsClient(_factory.CreateGrpcChannel());

            PermissionPage permissionPage = new PermissionPage
            {
                NumberPage = 0
            };

            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllAsync(permissionPage);
            });
            permissionPage.NumberPage = -10;
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.GetAllAsync(permissionPage);
            });
        }

        [Fact]
        public async Task AddPermission_EmptyName_Exception() {
            Permissions.PermissionsClient _client = new Permissions.PermissionsClient(_factory.CreateGrpcChannel());
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.AddAsync(new AddPermisson
                {
                    Name=""
                });
            });
        }

        [Fact]
        public async Task AddPermission_AddLengthNameMoreThanfour_Successfully()
        {
            Permissions.PermissionsClient _client = new Permissions.PermissionsClient(_factory.CreateGrpcChannel());
            var data = await _client.AddAsync(new AddPermisson
            {
                Name = "Full Permissions"
            });
            Assert.NotNull(data);
        }

        [Fact]
        public async Task UpdatePermission_EmptyName_Exception()
        {
            Permissions.PermissionsClient _client = new Permissions.PermissionsClient(_factory.CreateGrpcChannel());
            await _client.AddAsync(new AddPermisson
            {
                Name = "Full Controll"
            });
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.UpdateAsync(new UpdatePermission
                {
                    Id = 1,
                    Name = ""
                });
            });
        }

        [Fact]
        public async Task UpdatePermission_AddLengthNameMoreThanfour_Successfully()
        {
            Permissions.PermissionsClient _client = new Permissions.PermissionsClient(_factory.CreateGrpcChannel());
            var data = await _client.AddAsync(new AddPermisson
            {
                Name = "Full Permissions"
            });
            Assert.NotNull(data);
        }

        [Fact]
        public async Task DeletePermission_NegativeId_Exception()
        {
            Permissions.PermissionsClient _client = new Permissions.PermissionsClient(_factory.CreateGrpcChannel());
            
            await Assert.ThrowsAsync<RpcException>(async () =>
            {
                await _client.DeleteAsync(new DeletePermission
                {
                    Id = -1
                });
            });
        }

        [Fact]
        public async Task CheckPermission_Exists_Succesfully()
        {
            Permissions.PermissionsClient _client = new Permissions.PermissionsClient(_factory.CreateGrpcChannel());
            await _client.AddAsync(new AddPermisson
            {
                Name = "Full Controll"
            });

            var res = _client.CheckAsync(new PermissionId
            {
                Id = 1
            });
            Assert.NotNull(res);
        }
    }
}
