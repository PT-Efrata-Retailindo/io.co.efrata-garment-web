using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleSewingOuts.Configs
{
    public class GarmentSampleSewingOutItemConfig: IEntityTypeConfiguration<GarmentSampleSewingOutItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleSewingOutItemReadModel> builder)
        {
            builder.ToTable("GarmentSampleSewingOutItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSampleSewingOutIdentity)
                   .WithMany(a => a.GarmentSewingOutItem)
                   .HasForeignKey(a => a.SampleSewingOutId);

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
