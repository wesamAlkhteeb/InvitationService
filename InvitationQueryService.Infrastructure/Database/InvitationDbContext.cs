using InvitationQueryService.Domain.Entities;
using InvitationQueryService.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace InvitationQueryService.Infrastructure.Database
{
    public class InvitationDbContext : DbContext
    {
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<SubscriptorPermissionsEntity> SubscriptionPermissions { get; set; }
        public DbSet<SubscriptionsEntity> Subscriptions { get; set; }
        public DbSet<SubscriptorEntity> Subscriptors { get; set; }

        public InvitationDbContext(DbContextOptions<InvitationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptorConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptionPermissionsConfiguration());
        }
    }
}
