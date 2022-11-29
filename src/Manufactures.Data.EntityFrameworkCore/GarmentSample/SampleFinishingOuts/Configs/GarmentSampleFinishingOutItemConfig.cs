using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishingOuts.Configs
{
    public class GarmentSampleFinishingOutItemConfig : IEntityTypeConfiguration<GarmentSampleFinishingOutItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleFinishingOutItemReadModel> builder)
        {
            builder.ToTable("GarmentSampleFinishingOutItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSampleFinishingOutIdentity)
                   .WithMany(a => a.GarmentSampleFinishingOutItem)
                   .HasForeignKey(a => a.FinishingOutId);

            builder.Property(a => a.ProductCode)
               .HasMaxLength(25);
            builder.Property(a => a.ProductName)
               .HasMaxLength(100);
            builder.Property(a => a.DesignColor)
               .HasMaxLength(2000);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);

            builder.Property(a => a.UomUnit)
               .HasMaxLength(25);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
