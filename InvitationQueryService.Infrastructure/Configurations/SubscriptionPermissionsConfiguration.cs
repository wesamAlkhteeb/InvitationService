using InvitationQueryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvitationQueryService.Infrastructure.Configurations
{
    public class SubscriptionPermissionsConfiguration : IEntityTypeConfiguration<SubscriptorPermissionsEntity>
    {
        public void Configure(EntityTypeBuilder<SubscriptorPermissionsEntity> builder)
        {
        }
    }
}
