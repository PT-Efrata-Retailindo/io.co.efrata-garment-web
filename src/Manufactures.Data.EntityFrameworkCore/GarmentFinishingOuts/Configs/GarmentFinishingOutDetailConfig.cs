using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishingOuts.Configs
{
    public class GarmentFinishingOutDetailConfig : IEntityTypeConfiguration<GarmentFinishingOutDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentFinishingOutDetailReadModel> builder)
        {
            builder.ToTable("GarmentFinishingOutDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentFinishingOutItemIdentity)
               .WithMany(a => a.GarmentFinishingOutDetail)
               .HasForeignKey(a => a.FinishingOutItemId);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);
            builder.Property(a => a.UomUnit)
               .HasMaxLength(25);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
