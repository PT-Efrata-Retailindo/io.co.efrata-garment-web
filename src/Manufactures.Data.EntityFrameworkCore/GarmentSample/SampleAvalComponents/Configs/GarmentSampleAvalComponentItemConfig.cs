using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleAvalComponents.Configs
{
    public class GarmentSampleAvalComponentItemConfig : IEntityTypeConfiguration<GarmentSampleAvalComponentItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleAvalComponentItemReadModel> builder)
        {
            builder.ToTable("GarmentSampleAvalComponentItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.Property(p => p.Color).HasMaxLength(1000);
            builder.Property(p => p.SizeName).HasMaxLength(100);
            builder.Property(p => p.UId).HasMaxLength(255);

            builder.HasOne(w => w.GarmentSampleAvalComponent)
                .WithMany(h => h.Items)
                .HasForeignKey(f => f.SampleAvalComponentId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
