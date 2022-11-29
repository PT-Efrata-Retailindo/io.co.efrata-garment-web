using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingOuts.Configs
{
    public class GarmentSampleCuttingOutItemConfig : IEntityTypeConfiguration<GarmentSampleCuttingOutItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleCuttingOutItemReadModel> builder)
        {
            builder.ToTable("GarmentSampleCuttingOutItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSampleCuttingOutIdentity)
                   .WithMany(a => a.GarmentSampleCuttingOutItem)
                   .HasForeignKey(a => a.CuttingOutId);

            builder.Property(a => a.ProductCode)
               .HasMaxLength(25);
            builder.Property(a => a.ProductName)
               .HasMaxLength(100);
            builder.Property(a => a.DesignColor)
               .HasMaxLength(2000);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
