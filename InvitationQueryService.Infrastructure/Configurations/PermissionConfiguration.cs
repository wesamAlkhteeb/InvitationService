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
    public class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {

            builder.HasMany<SubscriptorPermissionsEntity>()
                .WithOne(x => x.Permissions)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
            

            builder.HasData(new PermissionEntity
            {
                Id = 1,
                Name = "Transfer"
            });
            builder.HasData(new PermissionEntity
            {
                Id = 2,
                Name = "PurchaseCards"
            });



        }
    }
}
