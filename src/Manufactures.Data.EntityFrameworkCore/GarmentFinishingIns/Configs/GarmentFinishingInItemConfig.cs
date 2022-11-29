using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishingIns.Configs
{
    public class GarmentFinishingInItemConfig : IEntityTypeConfiguration<GarmentFinishingInItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentFinishingInItemReadModel> builder)
        {
            builder.ToTable("GarmentFinishingInItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentFinishingIn)
                   .WithMany(a => a.Items)
                   .HasForeignKey(a => a.FinishingInId);

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
