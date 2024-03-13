using InvitationCommandService.Domain;
using InvitationCommandService.Domain.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvitationCommandService.Infrastructure.Configuration
{
    public class BaseEventConfigurations : IEntityTypeConfiguration<EventEntity>
    {
        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.HasIndex(e => new { e.AggregateId, e.Sequence }).IsUnique();
            builder.Property<string>("EventType").HasMaxLength(128);
            //builder.HasDiscriminator(x => x.Type)
            //    .HasValue<SendInvitationEventEntity>("SendEvent")
            //    .HasValue<CancelInvitationEventEntity>("CancelEvent")
            //    .HasValue<AcceptInvitationEventEntity>("AcceptEvent")
            //    .HasValue<RejectInvitationEventEntity>("RejectEvent")
            //    .HasValue<JoinInvitationEventEntity>("JoinEvent")
            //    .HasValue<RemoveInvitationEventEntity>("RemoveEvent")
            //    .HasValue<ChangePermissionsInvitationEventEntity>("ChangePermissionEvent")
            //    .HasValue<LeaveInvitationEventEntity>("LeaveEvent");

            builder.HasDiscriminator(x => x.Type)
                .HasValue<SendInvitationEventEntity>(EventType.SendEvent.ToString())
                .HasValue<CancelInvitationEventEntity>(EventType.CancelEvent.ToString())
                .HasValue<AcceptInvitationEventEntity>(EventType.AcceptEvent.ToString())
                .HasValue<RejectInvitationEventEntity>(EventType.RejectEvent.ToString())
                .HasValue<JoinInvitationEventEntity>(EventType.JoinEvent.ToString())
                .HasValue<RemoveInvitationEventEntity>(EventType.RemoveEvent.ToString())
                .HasValue<ChangePermissionsInvitationEventEntity>(EventType.ChangePermissionEvent.ToString())
                .HasValue<LeaveInvitationEventEntity>(EventType.LeaveEvent.ToString());
        }

    }
}
