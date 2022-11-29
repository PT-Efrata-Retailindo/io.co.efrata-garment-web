using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingOuts.Configs
{
    public class GarmentSewingOutItemConfig : IEntityTypeConfiguration<GarmentSewingOutItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSewingOutItemReadModel> builder)
        {
            builder.ToTable("GarmentSewingOutItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSewingOutIdentity)
                   .WithMany(a => a.GarmentSewingOutItem)
                   .HasForeignKey(a => a.SewingOutId);

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
