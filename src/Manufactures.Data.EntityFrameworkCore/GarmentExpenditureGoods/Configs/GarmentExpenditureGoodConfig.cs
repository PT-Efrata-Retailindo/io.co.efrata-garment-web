using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentExpenditureGoods.Configs
{
    public class GarmentExpenditureGoodConfig : IEntityTypeConfiguration<GarmentExpenditureGoodReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentExpenditureGoodReadModel> builder)
        {
            builder.ToTable("GarmentExpenditureGoods");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.ExpenditureGoodNo).HasMaxLength(25);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.Invoice).HasMaxLength(50);
            builder.Property(a => a.BuyerName).HasMaxLength(100);
            builder.Property(a => a.BuyerCode).HasMaxLength(25);
            builder.Property(a => a.ExpenditureType).HasMaxLength(25);

            builder.HasIndex(i => i.ExpenditureGoodNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
