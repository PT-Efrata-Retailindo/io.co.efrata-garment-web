using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingOuts.Configs
{
    public class GarmentSampleCuttingOutDetailConfig : IEntityTypeConfiguration<GarmentSampleCuttingOutDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleCuttingOutDetailReadModel> builder)
        {
            builder.ToTable("GarmentSampleCuttingOutDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSampleCuttingOutItemIdentity)
               .WithMany(a => a.GarmentSampleCuttingOutDetail)
               .HasForeignKey(a => a.CuttingOutItemId);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);
            builder.Property(a => a.Color)
               .HasMaxLength(1000);
            builder.Property(a => a.CuttingOutUomUnit)
               .HasMaxLength(10);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
