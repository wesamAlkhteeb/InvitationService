using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Infrastructure.ServiceBus
{
    public class AzureOptions
    {
        public required string TopicName { get; set; }
        public required string ConnectionString { get; set; }
        public required string SubscriptionName { get; set; }
    }
}
