using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Domain.Entities
{
    public class SubscriptorEntity
    {
        public int Id { get; set; }
        public int SubscriptorAccountId { get; set; }
        public int SubscriptionId { get; set; }
        public int Sequence { get; set; }
        public required string Status { get; set; }
        public SubscriptionsEntity? Subscriptions { get; set; }
    }
}
