using InvitationCommandService.Application.Abstraction;
using InvitationCommandService.Application.ServiceBus;
using InvitationCommandService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationCommandService.Infrastructure.Repository
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
