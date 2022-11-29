using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishedGoodStocks.Configs
{
    public class GarmentSampleFinishedGoodStockHistoryConfig : IEntityTypeConfiguration<GarmentSampleFinishedGoodStockHistoryReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleFinishedGoodStockHistoryReadModel> builder)
        {
            builder.ToTable("GarmentSampleFinishedGoodStockHistories");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.StockType).HasMaxLength(50);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(1000);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.SizeName).HasMaxLength(50);
            builder.Property(a => a.UomUnit).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
