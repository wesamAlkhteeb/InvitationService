using Grpc.Core;
using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Infrastructure.Database;
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

            var data = await _client.GetAllAsync(new PermissionPage
            {
                NumberPage = 1
            });
            Assert.NotNull(data);
            Assert.Equal(2, data.Permission.Count());
        }

        [Fact]
        public async Task GetAll_NegativePage_Exception() {
            Permissions.PermissionsClient _client = new Permissions.PermissionsClient(_factory.CreateGrpcChannel());

            PermissionPage permissionPage = new PermissionPage
            {
                NumberPage = -1
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
        public async Task CheckPermission_Exists_Succesfully()
        {
            Permissions.PermissionsClient _client = new Permissions.PermissionsClient(_factory.CreateGrpcChannel());
            var scope = _factory.Services.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<InvitationDbContext>();
            //I seed to permissions so this Id must be equal 3.
            var permission = new PermissionEntity
            {
                Name = "TTT"
            };
            database.Permissions.Add(permission);
            await database.SaveChangesAsync();
            var res = await _client.CheckAsync(new PermissionId
            {
                Id = permission.Id
            });
            Assert.NotNull(res);
        }
    }
}
