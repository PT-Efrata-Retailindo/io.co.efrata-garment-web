using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleAvalComponents.Configs
{
    public class GarmentSampleAvalComponentConfig : IEntityTypeConfiguration<GarmentSampleAvalComponentReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleAvalComponentReadModel> builder)
        {
            builder.ToTable("GarmentSampleAvalComponents");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.SampleAvalComponentNo).HasMaxLength(25);
            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);
            builder.Property(p => p.SampleAvalComponentType).HasMaxLength(25);
            builder.Property(p => p.RONo).HasMaxLength(25);
            builder.Property(p => p.Article).HasMaxLength(1000);
            builder.Property(p => p.ComodityCode).HasMaxLength(25);
            builder.Property(p => p.ComodityName).HasMaxLength(100);

            builder.HasIndex(i => i.SampleAvalComponentNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
