using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconSewings.Config
{
    public class GarmentServiceSubconSewingDetailConfig : IEntityTypeConfiguration<GarmentServiceSubconSewingDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconSewingDetailReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconSewingDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSubconSewingItemIdentity)
                   .WithMany(a => a.GarmentServiceSubconSewingDetail)
                   .HasForeignKey(a => a.ServiceSubconSewingItemId);

            builder.Property(a => a.ProductCode)
               .HasMaxLength(25);
            builder.Property(a => a.ProductName)
               .HasMaxLength(100);
            builder.Property(a => a.DesignColor)
               .HasMaxLength(2000);

            builder.Property(a => a.UomUnit)
               .HasMaxLength(25);

            builder.Property(a => a.Remark)
               .HasMaxLength(2000);

            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
