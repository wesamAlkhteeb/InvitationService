using InvitationQueryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvitationQueryService.Infrastructure.Configurations
{
    public class SubscriptorConfiguration : IEntityTypeConfiguration<SubscriptorEntity>
    {
        public void Configure(EntityTypeBuilder<SubscriptorEntity> builder)
        {
            //builder.Property(x=>x.Sequence).IsUnicode(true);
            builder.HasMany<SubscriptorPermissionsEntity>()
                .WithOne(x => x.Subscriptor)
                .HasForeignKey(x => x.SubscriptorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.Sequence).IsConcurrencyToken();
        }
    }
}
