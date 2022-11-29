using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleSewingOuts.Configs
{
    public class GarmentSampleSewingOutDetailConfig : IEntityTypeConfiguration<GarmentSampleSewingOutDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleSewingOutDetailReadModel> builder)
        {
            builder.ToTable("GarmentSampleSewingOutDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSampleSewingOutItemIdentity)
               .WithMany(a => a.GarmentSampleSewingOutDetail)
               .HasForeignKey(a => a.SampleSewingOutItemId);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);
            builder.Property(a => a.UomUnit)
               .HasMaxLength(25);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
