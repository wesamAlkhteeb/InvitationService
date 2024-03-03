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
    public class SubscriptorConfiguration : IEntityTypeConfiguration<SubscriptorEntity>
    {
        public void Configure(EntityTypeBuilder<SubscriptorEntity> builder)
        {
            //builder.Property(x=>x.Sequence).IsUnicode(true);
            builder.HasMany<SubscriptorPermissionsEntity>()
                .WithOne(x => x.Subscriptor)
                .HasForeignKey(x => x.SubscriptorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
