using InvitationQueryService.Domain;
using InvitationQueryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
                AccountId = 1,
                Type = SubscriptionType.A.ToString()
            });
        }
    }
}
