using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleDeliveryReturns.Config
{
    public class GarmentSampleDeliveryReturnConfig : IEntityTypeConfiguration<GarmentSampleDeliveryReturnReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleDeliveryReturnReadModel> builder)
        {
            builder.ToTable("GarmentSampleDeliveryReturns");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.DRNo)
               .HasMaxLength(25);
            builder.Property(a => a.RONo)
               .HasMaxLength(100);
            builder.Property(a => a.Article)
               .HasMaxLength(1000);
            builder.Property(a => a.UnitDONo)
               .HasMaxLength(100);
            builder.Property(a => a.ReturnType)
               .HasMaxLength(25);
            builder.Property(a => a.UnitCode)
               .HasMaxLength(25);
            builder.Property(a => a.UnitName)
               .HasMaxLength(100);
            builder.Property(a => a.StorageCode)
               .HasMaxLength(25);
            builder.Property(a => a.StorageName)
               .HasMaxLength(100);

            builder.HasIndex(i => i.DRNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
