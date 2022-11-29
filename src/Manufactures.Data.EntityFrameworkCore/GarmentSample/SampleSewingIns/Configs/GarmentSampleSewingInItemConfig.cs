using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleSewingIns.Configs
{
    public class GarmentSampleSewingInItemConfig : IEntityTypeConfiguration<GarmentSampleSewingInItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleSewingInItemReadModel> builder)
        {
            builder.ToTable("GarmentSampleSewingInItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSampleSewingInIdentity)
                   .WithMany(a => a.GarmentSampleSewingInItem)
                   .HasForeignKey(a => a.SewingInId);

            builder.Property(a => a.ProductCode).HasMaxLength(25);
            builder.Property(a => a.ProductName).HasMaxLength(100);
            builder.Property(a => a.DesignColor).HasMaxLength(2000);
            builder.Property(a => a.SizeName).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(10);
            builder.Property(a => a.Color).HasMaxLength(1000);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
