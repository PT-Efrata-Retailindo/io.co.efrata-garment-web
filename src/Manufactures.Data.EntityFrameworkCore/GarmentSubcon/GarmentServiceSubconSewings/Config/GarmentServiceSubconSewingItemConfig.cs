using System;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconSewings.Config
{
    public class GarmentServiceSubconSewingItemConfig : IEntityTypeConfiguration<GarmentServiceSubconSewingItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconSewingItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconSewingItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSubconSewingIdentity)
                   .WithMany(a => a.GarmentServiceSubconSewingItem)
                   .HasForeignKey(a => a.ServiceSubconSewingId);


            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.BuyerName).HasMaxLength(100);
            builder.Property(a => a.BuyerCode).HasMaxLength(25);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
