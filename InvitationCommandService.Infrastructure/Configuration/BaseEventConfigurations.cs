using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InvitationCommandService.Domain.Entities.Events;

namespace InvitationCommandService.Infrastructure.Configuration
{
    public class BaseEventConfigurations : IEntityTypeConfiguration<EventEntity>
    {
        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.HasIndex(e => new { e.AggregateId, e.Sequence }).IsUnique();
            builder.Property<string>("EventType").HasMaxLength(128);
            builder.HasDiscriminator(x => x.Type)
                .HasValue<SendInvitationEventEntity>("SendEvent")
                .HasValue<CancelInvitationEventEntity>("CancelEvent")
                .HasValue<AcceptInvitationEventEntity>("AcceptEvent")
                .HasValue<RejectInvitationEventEntity>("RejectEvent")
                .HasValue<JoinInvitationEventEntity>("JoinEvent")
                .HasValue<RemoveInvitationEventEntity>("RemoveEvent")
                .HasValue<ChangePermissionsInvitationEventEntity>("ChangePermissionEvent")
                .HasValue<LeaveInvitationEventEntity>("LeaveEvent");
        }
    }
}
