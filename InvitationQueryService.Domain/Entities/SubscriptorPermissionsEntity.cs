using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Domain.Entities
{
    public class SubscriptorPermissionsEntity
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int PermissionId { get; set; }
        public int SubscriptorId { get; set; }
        public PermissionEntity? Permissions { get; set; }
        public SubscriptionsEntity? Subscriptions { get; set; }
        public SubscriptorEntity? Subscriptor { get; set; }
    }
}
