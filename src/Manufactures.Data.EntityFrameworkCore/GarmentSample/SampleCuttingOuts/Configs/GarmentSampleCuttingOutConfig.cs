using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingOuts.Configs
{
    public class GarmentSampleCuttingOutConfig : IEntityTypeConfiguration<GarmentSampleCuttingOutReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleCuttingOutReadModel> builder)
        {
            builder.ToTable("GarmentSampleCuttingOuts");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.CutOutNo).HasMaxLength(25);
            builder.Property(a => a.CuttingOutType).HasMaxLength(25);
            builder.Property(a => a.UnitFromCode).HasMaxLength(25);
            builder.Property(a => a.UnitFromName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(1000);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.POSerialNumber)
               .HasMaxLength(100);

            builder.HasIndex(i => i.CutOutNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
