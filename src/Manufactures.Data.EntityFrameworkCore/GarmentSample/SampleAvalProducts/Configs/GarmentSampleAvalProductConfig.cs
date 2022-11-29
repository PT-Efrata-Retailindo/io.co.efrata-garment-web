using Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleAvalProducts.Configs
{
    public class GarmentSampleAvalProductConfig : IEntityTypeConfiguration<GarmentSampleAvalProductReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleAvalProductReadModel> builder)
        {
            builder.ToTable("GarmentSampleAvalProducts");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.RONo).HasMaxLength(100);
            builder.Property(a => a.Article).HasMaxLength(1000);

            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
