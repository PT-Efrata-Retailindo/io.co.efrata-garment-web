using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentComodityPrices.Configs
{
    public class GarmentComodityPriceConfig : IEntityTypeConfiguration<GarmentComodityPriceReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentComodityPriceReadModel> builder)
        {
            builder.ToTable("GarmentComodityPrices");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);
            builder.Property(p => p.ComodityCode).HasMaxLength(25);
            builder.Property(p => p.ComodityName).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    
    }
}
