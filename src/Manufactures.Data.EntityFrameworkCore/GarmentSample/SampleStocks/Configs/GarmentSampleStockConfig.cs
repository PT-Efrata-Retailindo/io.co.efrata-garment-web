using Manufactures.Domain.GarmentSample.SampleStocks.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleStocks.Configs
{
    public class GarmentSampleStockConfig : IEntityTypeConfiguration<GarmentSampleStockReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleStockReadModel> builder)
        {
            builder.ToTable("GarmentSampleStocks");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.SampleStockNo).HasMaxLength(25);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(1000);
            builder.Property(a => a.ArchiveType).HasMaxLength(25);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.SizeName).HasMaxLength(50);
            builder.Property(a => a.UomUnit).HasMaxLength(100);

            builder.HasIndex(i => i.SampleStockNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
