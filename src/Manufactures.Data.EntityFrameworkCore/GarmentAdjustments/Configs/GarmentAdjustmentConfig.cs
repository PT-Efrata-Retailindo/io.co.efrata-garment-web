using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentAdjustments.Configs
{
    public class GarmentAdjustmentConfig : IEntityTypeConfiguration<GarmentAdjustmentReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentAdjustmentReadModel> builder)
        {
            builder.ToTable("GarmentAdjustments");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.AdjustmentNo).HasMaxLength(25);
            builder.Property(p => p.AdjustmentType).HasMaxLength(25);
            builder.Property(p => p.RONo).HasMaxLength(25);
            builder.Property(p => p.Article).HasMaxLength(50);
            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);
            builder.Property(p => p.ComodityName).HasMaxLength(500);
            builder.Property(p => p.ComodityCode).HasMaxLength(100);
            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);
            builder.Property(p => p.AdjustmentDesc).HasMaxLength(255);

            builder.HasIndex(i => i.AdjustmentNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
