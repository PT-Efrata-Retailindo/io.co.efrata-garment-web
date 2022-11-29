using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentAdjustments.Configs
{
    public class GarmentAdjustmentItemConfig : IEntityTypeConfiguration<GarmentAdjustmentItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentAdjustmentItemReadModel> builder)
        {
            builder.ToTable("GarmentAdjustmentItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(50);
            builder.Property(p => p.ProductName).HasMaxLength(500);
            builder.Property(p => p.Color).HasMaxLength(1000);
            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.Property(p => p.SizeName).HasMaxLength(50);
            builder.Property(p => p.UomUnit).HasMaxLength(50);

            builder.HasOne(w => w.GarmentAdjustment)
                .WithMany(h => h.Items)
                .HasForeignKey(f => f.AdjustmentId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
