
using InvitationCommandService.Domain.Entities;
using InvitationCommandService.Domain.Entities.Data;
using InvitationCommandService.Domain.Entities.Events;
using InvitationCommandService.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace InvitationCommandService.Database
{
    public class InvitationDbContext : DbContext
    {

        public DbSet<EventEntity> Events { get; set; }
        public DbSet<OutboxMessageEntity> Outboxes { get; set; }
        public InvitationDbContext(DbContextOptions<InvitationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageConfigurations());
            modelBuilder.ApplyConfiguration(new BaseEventConfigurations());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<SendInvitationEventEntity, InvitationData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<JoinInvitationEventEntity, InvitationData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<ChangePermissionsInvitationEventEntity, InvitationData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<AcceptInvitationEventEntity, InvitationInfoData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<CancelInvitationEventEntity, InvitationInfoData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<RejectInvitationEventEntity, InvitationInfoData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<RemoveInvitationEventEntity, InvitationInfoData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<LeaveInvitationEventEntity, InvitationInfoData>());
        }
    }
}
