using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingIns.Configs
{
    public class GarmentSampleCuttingInDetailConfig : IEntityTypeConfiguration<GarmentSampleCuttingInDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleCuttingInDetailReadModel> builder)
        {
            builder.ToTable("GarmentSampleCuttingInDetails");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.Property(p => p.FabricType).HasMaxLength(25);
            builder.Property(p => p.PreparingUomUnit).HasMaxLength(10);
            builder.Property(p => p.CuttingInUomUnit).HasMaxLength(10);
            builder.Property(p => p.Color).HasMaxLength(1000);
            builder.HasOne(w => w.GarmentSampleCuttingInItem)
                .WithMany(h => h.Details)
                .HasForeignKey(f => f.CutInItemId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
