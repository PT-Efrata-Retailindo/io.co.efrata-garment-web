using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconCuttings.Configs
{
    public class GarmentServiceSubconCuttingSizeConfig : IEntityTypeConfiguration<GarmentServiceSubconCuttingSizeReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconCuttingSizeReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconCuttingSizes");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.SizeName).HasMaxLength(100);
            builder.Property(p => p.UomUnit).HasMaxLength(10);
            builder.Property(p => p.Color).HasMaxLength(2000);
            builder.HasOne(w => w.GarmentServiceSubconCuttingDetail)
                .WithMany(h => h.GarmentServiceSubconCuttingSizes)
                .HasForeignKey(f => f.ServiceSubconCuttingDetailId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
