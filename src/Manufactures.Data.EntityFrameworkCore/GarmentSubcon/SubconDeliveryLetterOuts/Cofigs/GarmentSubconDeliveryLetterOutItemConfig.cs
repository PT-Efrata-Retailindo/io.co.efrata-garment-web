using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.SubconDeliveryLetterOuts.Cofigs
{
    public class GarmentSubconDeliveryLetterOutItemConfig : IEntityTypeConfiguration<GarmentSubconDeliveryLetterOutItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconDeliveryLetterOutItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconDeliveryLetterOutItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.Property(p => p.ProductRemark).HasMaxLength(2000);
            builder.Property(p => p.UomUnit).HasMaxLength(50);
            builder.Property(p => p.UomOutUnit).HasMaxLength(50);
            builder.Property(p => p.FabricType).HasMaxLength(255);
            builder.Property(p => p.RONo).HasMaxLength(25);
            builder.Property(p => p.POSerialNumber).HasMaxLength(50);
            builder.Property(p => p.SubconNo).HasMaxLength(50);
            builder.Property(p => p.UomSatuanUnit).HasMaxLength(10);
            builder.HasOne(w => w.GarmentSubconDeliveryLetterOut)
                .WithMany(h => h.GarmentSubconDeliveryLetterOutItem)
                .HasForeignKey(f => f.SubconDeliveryLetterOutId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
