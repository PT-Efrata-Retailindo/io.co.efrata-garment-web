using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishingIns.Configs
{
    public class GarmentSampleFinishingInConfig : IEntityTypeConfiguration<GarmentSampleFinishingInReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleFinishingInReadModel> builder)
        {
            builder.ToTable("GarmentSampleFinishingIns");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.FinishingInNo).HasMaxLength(25);
            builder.Property(a => a.UnitFromCode).HasMaxLength(25);
            builder.Property(a => a.UnitFromName).HasMaxLength(100);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(1000);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);

            builder.Property(a => a.DONo).HasMaxLength(100);
            builder.Property(a => a.SubconType).HasMaxLength(100);

            builder.HasIndex(i => i.FinishingInNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
