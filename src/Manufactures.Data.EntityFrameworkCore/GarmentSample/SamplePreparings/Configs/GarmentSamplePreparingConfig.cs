using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SamplePreparings.Configs
{
    public class GarmentSamplePreparingConfig : IEntityTypeConfiguration<GarmentSamplePreparingReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSamplePreparingReadModel> builder)
        {
            builder.ToTable("GarmentSamplePreparings");
            builder.HasKey(e => e.Identity);

            builder.Property(o => o.UENNo)
               .HasMaxLength(100);
            builder.Property(o => o.UnitCode)
               .HasMaxLength(25);
            builder.Property(o => o.UnitName)
               .HasMaxLength(100);
            builder.Property(o => o.RONo)
               .HasMaxLength(100);
            builder.Property(o => o.Article)
               .HasMaxLength(1000);
            builder.Property(o => o.BuyerCode)
               .HasMaxLength(100);
            builder.Property(o => o.BuyerName)
               .HasMaxLength(500);
            builder.Property(o => o.UId)
                .HasMaxLength(255);
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
