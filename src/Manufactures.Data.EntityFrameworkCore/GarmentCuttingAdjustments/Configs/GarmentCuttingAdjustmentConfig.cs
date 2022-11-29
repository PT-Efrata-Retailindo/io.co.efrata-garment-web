using Manufactures.Domain.GarmentCuttingAdjustments.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingAdjustments.Configs
{
    public class GarmentCuttingAdjustmentConfig : IEntityTypeConfiguration<GarmentCuttingAdjustmentReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentCuttingAdjustmentReadModel> builder)
        {
            builder.ToTable("GarmentCuttingAdjustments");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.CutInNo).HasMaxLength(25);
            builder.Property(p => p.RONo).HasMaxLength(25);
            builder.Property(p => p.AdjustmentNo).HasMaxLength(25);
            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);
            builder.Property(p => p.AdjustmentDesc).HasMaxLength(4000);
            builder.HasIndex(i => i.AdjustmentNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}