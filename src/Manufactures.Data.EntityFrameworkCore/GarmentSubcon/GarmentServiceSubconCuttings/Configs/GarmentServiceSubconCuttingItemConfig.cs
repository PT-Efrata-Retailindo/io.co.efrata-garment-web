using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconCuttings.Configs
{
    public class GarmentServiceSubconCuttingItemConfig : IEntityTypeConfiguration<GarmentServiceSubconCuttingItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconCuttingItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconCuttingItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.RONo).HasMaxLength(25);
            builder.Property(p => p.Article).HasMaxLength(50);
            builder.Property(p => p.ComodityName).HasMaxLength(500);
            builder.Property(p => p.ComodityCode).HasMaxLength(255);
            builder.HasOne(w => w.GarmentServiceSubconCutting)
                .WithMany(h => h.GarmentServiceSubconCuttingItem)
                .HasForeignKey(f => f.ServiceSubconCuttingId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}