using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleRequests.Configs
{
    public class GarmentSampleRequestProductConfig : IEntityTypeConfiguration<GarmentSampleRequestProductReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleRequestProductReadModel> builder)
        {
            builder.ToTable("GarmentSampleRequestProducts");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.Color).HasMaxLength(500);
            builder.Property(p => p.Fabric).HasMaxLength(500);
            builder.Property(p => p.SizeDescription).HasMaxLength(500);
            builder.Property(p => p.Style).HasMaxLength(255);
            builder.Property(a => a.SizeName).HasMaxLength(50);

            builder.HasOne(w => w.GarmentSampleRequest)
                .WithMany(h => h.SampleProduct)
                .HasForeignKey(f => f.SampleRequestId);
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
