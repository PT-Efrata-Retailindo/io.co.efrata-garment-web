using Manufactures.Domain.MonitoringProductionStockFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentMonitoringProductionStockFlows.Configs
{
    public class GarmentBalanceMonitoringProductionStockFlowConfig : IEntityTypeConfiguration<GarmentBalanceMonitoringProductionStockReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentBalanceMonitoringProductionStockReadModel> builder)
        {
            builder.ToTable("GarmentBalanceProductionStocks");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.Ro).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(100);
            builder.Property(a => a.BuyerCode).HasMaxLength(50);
            builder.Property(a => a.Comodity).HasMaxLength(50);
           
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
