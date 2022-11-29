using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishedGoodStocks.Configs
{
    public class GarmentFinishedGoodStockHistoryConfig : IEntityTypeConfiguration<GarmentFinishedGoodStockHistoryReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentFinishedGoodStockHistoryReadModel> builder)
        {
            builder.ToTable("GarmentFinishedGoodStockHistories");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.StockType).HasMaxLength(50);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
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
