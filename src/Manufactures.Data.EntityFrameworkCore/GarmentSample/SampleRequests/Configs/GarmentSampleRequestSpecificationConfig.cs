using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleRequests.Configs
{
    public class GarmentSampleRequestSpecificationConfig : IEntityTypeConfiguration<GarmentSampleRequestSpecificationReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleRequestSpecificationReadModel> builder)
        {
            builder.ToTable("GarmentSampleRequestSpecifications");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.Inventory).HasMaxLength(100);
            builder.Property(p => p.SpecificationDetail).HasMaxLength(500);
            builder.Property(p => p.Remark).HasMaxLength(1000);
            builder.Property(p => p.UomUnit).HasMaxLength(50);

            builder.HasOne(w => w.GarmentSampleRequest)
                .WithMany(h => h.SampleSpecification)
                .HasForeignKey(f => f.SampleRequestId);
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
