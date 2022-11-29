using Manufactures.Domain.GarmentCuttingAdjustments.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingAdjustments.Configs
{
    public class GarmentCuttingAdjustmentItemConfig : IEntityTypeConfiguration<GarmentCuttingAdjustmentItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentCuttingAdjustmentItemReadModel> builder)
        {
            builder.ToTable("GarmentCuttingAdjustmentItems");
            builder.HasKey(e => e.Identity);

            builder.HasOne(w => w.GarmentAdjustmentCutting)
                .WithMany(h => h.Items)
                .HasForeignKey(f => f.AdjustmentCuttingId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
