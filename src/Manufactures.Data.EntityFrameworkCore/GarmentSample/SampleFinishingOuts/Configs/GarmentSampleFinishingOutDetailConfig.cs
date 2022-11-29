using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishingOuts.Configs
{
    public class GarmentSampleFinishingOutDetailConfig : IEntityTypeConfiguration<GarmentSampleFinishingOutDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleFinishingOutDetailReadModel> builder)
        {
            builder.ToTable("GarmentSampleFinishingOutDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSampleFinishingOutItemIdentity)
               .WithMany(a => a.GarmentSampleFinishingOutDetail)
               .HasForeignKey(a => a.FinishingOutItemId);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);
            builder.Property(a => a.UomUnit)
               .HasMaxLength(25);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
