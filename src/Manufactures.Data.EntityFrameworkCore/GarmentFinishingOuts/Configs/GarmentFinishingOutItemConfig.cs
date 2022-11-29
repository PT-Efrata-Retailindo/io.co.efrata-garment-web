using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishingOuts.Configs
{
    public class GarmentFinishingOutItemConfig : IEntityTypeConfiguration<GarmentFinishingOutItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentFinishingOutItemReadModel> builder)
        {
            builder.ToTable("GarmentFinishingOutItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentFinishingOutIdentity)
                   .WithMany(a => a.GarmentFinishingOutItem)
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
