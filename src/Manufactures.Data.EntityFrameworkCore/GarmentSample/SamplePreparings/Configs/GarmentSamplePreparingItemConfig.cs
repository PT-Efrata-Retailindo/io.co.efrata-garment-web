using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SamplePreparings.Configs
{
    public class GarmentSamplePreparingItemConfig : IEntityTypeConfiguration<GarmentSamplePreparingItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSamplePreparingItemReadModel> builder)
        {
            builder.ToTable("GarmentSamplePreparingItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSamplePreparingIdentity)
               .WithMany(a => a.GarmentSamplePreparingItem)
               .HasForeignKey(a => a.GarmentSamplePreparingId);

            builder.Property(o => o.ProductCode)
               .HasMaxLength(25);
            builder.Property(o => o.ProductName)
               .HasMaxLength(100);
            builder.Property(o => o.UomUnit)
               .HasMaxLength(100);
            builder.Property(o => o.DesignColor)
               .HasMaxLength(2000);
            builder.Property(o => o.FabricType)
               .HasMaxLength(100);
            builder.Property(o => o.ROSource)
               .HasMaxLength(100);
            builder.Property(o => o.UId)
                .HasMaxLength(255);
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
