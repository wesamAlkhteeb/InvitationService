using InvitationQueryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvitationQueryService.Infrastructure.Configurations
{
    public class SubscriptionPermissionsConfiguration : IEntityTypeConfiguration<SubscriptorPermissionsEntity>
    {
        public void Configure(EntityTypeBuilder<SubscriptorPermissionsEntity> builder)
        {
        }
    }
}
