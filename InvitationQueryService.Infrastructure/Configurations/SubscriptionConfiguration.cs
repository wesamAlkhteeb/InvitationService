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
    public class SubscriptionConfiguration : IEntityTypeConfiguration<SubscriptionsEntity>
    {
        public void Configure(EntityTypeBuilder<SubscriptionsEntity> builder)
        {
            builder.HasMany<SubscriptorPermissionsEntity>()
                    .WithOne(x => x.Subscriptions)
                    .HasForeignKey(x => x.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany<SubscriptorEntity>()
                .WithOne(x => x.Subscriptions)
                .HasForeignKey(x => x.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasData(new SubscriptionsEntity
            {
                Id = 1,
                AccountId = 1
            });
        }
    }
}
