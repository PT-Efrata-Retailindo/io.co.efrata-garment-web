using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconFabricWashes.Config
{
    public class GarmentServiceSubconFabricWashDetailConfig : IEntityTypeConfiguration<GarmentServiceSubconFabricWashDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconFabricWashDetailReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconFabricWashDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSubconFabricWashItemIdentity)
                   .WithMany(a => a.GarmentServiceSubconFabricWashDetail)
                   .HasForeignKey(a => a.ServiceSubconFabricWashItemId);

            builder.Property(a => a.ProductCode).HasMaxLength(25);
            builder.Property(a => a.ProductName).HasMaxLength(100);
            builder.Property(a => a.ProductRemark).HasMaxLength(1000);
            builder.Property(a => a.DesignColor).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
