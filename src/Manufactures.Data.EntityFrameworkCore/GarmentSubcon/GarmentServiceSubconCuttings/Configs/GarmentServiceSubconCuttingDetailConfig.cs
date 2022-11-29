using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconCuttings.Configs
{
    public class GarmentServiceSubconCuttingDetailConfig : IEntityTypeConfiguration<GarmentServiceSubconCuttingDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconCuttingDetailReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconCuttingDetails");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.HasOne(w => w.GarmentServiceSubconCuttingItem)
                .WithMany(h => h.GarmentServiceSubconCuttingDetail)
                .HasForeignKey(f => f.ServiceSubconCuttingItemId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}