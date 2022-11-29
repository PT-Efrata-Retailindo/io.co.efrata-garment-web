using Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleAvalProducts.Configs
{
    public class GarmentSampleAvalProductItemConfig : IEntityTypeConfiguration<GarmentSampleAvalProductItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleAvalProductItemReadModel> builder)
        {
            builder.ToTable("GarmentSampleAvalProductItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSampleAvalProductIdentity)
               .WithMany(a => a.GarmentSampleAvalProductItem)
               .HasForeignKey(a => a.APId);

            builder.Property(o => o.ProductCode).HasMaxLength(25);
            builder.Property(o => o.ProductName).HasMaxLength(100);
            builder.Property(o => o.DesignColor).HasMaxLength(2000);
            builder.Property(o => o.UomUnit).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();

        }
    }
}
