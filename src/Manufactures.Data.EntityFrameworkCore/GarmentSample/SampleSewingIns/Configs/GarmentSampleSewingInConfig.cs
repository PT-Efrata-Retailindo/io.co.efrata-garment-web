using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleSewingIns.Configs
{
    public class GarmentSampleSewingInConfig : IEntityTypeConfiguration<GarmentSampleSewingInReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleSewingInReadModel> builder)
        {
            builder.ToTable("GarmentSampleSewingIns");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.SewingInNo).HasMaxLength(25);
            builder.Property(a => a.SewingFrom).HasMaxLength(25);
            builder.Property(a => a.CuttingOutNo).HasMaxLength(25);
            builder.Property(a => a.UnitFromCode).HasMaxLength(25);
            builder.Property(a => a.UnitFromName).HasMaxLength(100);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(1000);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);

            builder.HasIndex(i => i.SewingInNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
