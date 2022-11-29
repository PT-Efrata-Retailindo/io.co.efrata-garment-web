using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingIns.Configs
{
    public class GarmentSampleCuttingInItemConfig : IEntityTypeConfiguration<GarmentSampleCuttingInItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleCuttingInItemReadModel> builder)
        {
            builder.ToTable("GarmentSampleCuttingInItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.UENNo).HasMaxLength(100);
            builder.Property(p => p.SewingOutNo).HasMaxLength(50);
            builder.HasOne(w => w.GarmentSampleCuttingIn)
                .WithMany(h => h.Items)
                .HasForeignKey(f => f.CutInId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
