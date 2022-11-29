using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishedGoodStocks.Configs
{
    public class GarmentFinishedGoodStockConfig : IEntityTypeConfiguration<GarmentFinishedGoodStockReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentFinishedGoodStockReadModel> builder)
        {
            builder.ToTable("GarmentFinishedGoodStocks");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.FinishedGoodStockNo).HasMaxLength(25);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.SizeName).HasMaxLength(50);
            builder.Property(a => a.UomUnit).HasMaxLength(100);

            builder.HasIndex(i => i.FinishedGoodStockNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
