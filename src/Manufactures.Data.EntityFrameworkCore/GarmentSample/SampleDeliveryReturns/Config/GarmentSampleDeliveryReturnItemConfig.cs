using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleDeliveryReturns.Config
{
    public class GarmentSampleDeliveryReturnItemConfig : IEntityTypeConfiguration<GarmentSampleDeliveryReturnItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleDeliveryReturnItemReadModel> builder)
        {
            builder.ToTable("GarmentSampleDeliveryReturnItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSampleDeliveryReturnIdentity)
               .WithMany(a => a.GarmentSampleDeliveryReturnItem)
               .HasForeignKey(a => a.DRId);

            builder.Property(a => a.ProductCode)
               .HasMaxLength(25);
            builder.Property(a => a.ProductName)
               .HasMaxLength(100);
            builder.Property(a => a.DesignColor)
               .HasMaxLength(2000);
            builder.Property(a => a.RONo)
               .HasMaxLength(100);
            builder.Property(a => a.UomUnit)
               .HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
