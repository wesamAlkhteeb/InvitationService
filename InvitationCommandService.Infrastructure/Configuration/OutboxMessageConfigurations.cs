using InvitationCommandService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvitationCommandService.Infrastructure.Configuration
{
    public class OutboxMessageConfigurations : IEntityTypeConfiguration<OutboxMessageEntity>
    {
        public void Configure(EntityTypeBuilder<OutboxMessageEntity> builder)
        {
            builder.HasOne(x => x.Event)
                .WithOne()
                .HasForeignKey<OutboxMessageEntity>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
