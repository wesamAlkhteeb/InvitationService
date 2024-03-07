using InvitationQueryService.Application.Abstraction;
using InvitationQueryService.Application.ServiceBus;

namespace InvitationQueryService.Infrastructure.Repository
{
    public class ServiceBusRepository : IServiceBusRepository
    {
        private readonly ServiceBusPublisher serviceBus;

        public ServiceBusRepository(ServiceBusPublisher serviceBus)
        {
            this.serviceBus = serviceBus;
        }

        public async Task PublicMessage()
        {
            await this.serviceBus.StartPublishing();
        }
    }
}
