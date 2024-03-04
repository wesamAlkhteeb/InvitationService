
using InvitationQueryService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InvintionCommandTest.Helper
{
    public static class ServiceCollectionExtensions
    {
        public static void ReplaceWithInMemoryDatabase(this IServiceCollection services)
        {
            var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<InvitationDbContext>));

            services.Remove(descriptor);

            var dbName = Guid.NewGuid().ToString();

            services.AddDbContext<InvitationDbContext>(options => options.UseInMemoryDatabase(dbName));
        }
    }
}