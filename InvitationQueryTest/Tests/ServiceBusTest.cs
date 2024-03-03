using InvitationCommandService.Application.ServiceBus;
using InvitationQueryService.Infrastructure.ServiceBus;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace InvitationQueryTest.Test
{
    public class ServiceBusTest:IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory;

        public ServiceBusTest(WebApplicationFactory<Program> factory,AzureOptions azure, IServiceProvider service, ILogger<InvitationListener> logger)
        {
            this.factory = factory;
            InvitationListener invitation = new InvitationListener(azure, service, logger);
            
        }

        [Fact]
        public void Test1()
        {

        }
    }
}