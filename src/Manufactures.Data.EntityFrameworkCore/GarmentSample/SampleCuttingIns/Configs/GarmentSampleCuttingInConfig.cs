using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingIns.Configs
{
    public class GarmentSampleCuttingInConfig : IEntityTypeConfiguration<GarmentSampleCuttingInReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleCuttingInReadModel> builder)
        {
            builder.ToTable("GarmentSampleCuttingIns");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.CutInNo).HasMaxLength(25);
            builder.Property(p => p.CuttingType).HasMaxLength(25);
            builder.Property(p => p.CuttingFrom).HasMaxLength(25);
            builder.Property(p => p.RONo).HasMaxLength(25);
            builder.Property(p => p.Article).HasMaxLength(1000);
            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);
            builder.HasIndex(i => i.CutInNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
