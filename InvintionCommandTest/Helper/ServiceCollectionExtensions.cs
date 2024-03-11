using Azure.Messaging.ServiceBus;
using InvitationCommandService.Database;
using InvitationCommandService.Infrastructure.ServiceBus;
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
        public static void RejectServiceBus(this IServiceCollection services)
        {

            AzureOptions azure = services.BuildServiceProvider().GetService<AzureOptions>()!;
            
            var descriptor = services.Single(d => d.ServiceType == typeof(AzureOptions));

            services.Remove(descriptor);

            azure.IsNeedToSend = false;

            services.AddSingleton<AzureOptions>(azure);

        }
    }
}