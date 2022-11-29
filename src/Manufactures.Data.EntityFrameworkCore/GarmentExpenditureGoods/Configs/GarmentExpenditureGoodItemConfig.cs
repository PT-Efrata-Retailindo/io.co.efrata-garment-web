using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentExpenditureGoods.Configs
{
    public class GarmentExpenditureGoodItemConfig : IEntityTypeConfiguration<GarmentExpenditureGoodItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentExpenditureGoodItemReadModel> builder)
        {
            builder.ToTable("GarmentExpenditureGoodItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentExpenditureGood)
                   .WithMany(a => a.Items)
                   .HasForeignKey(a => a.ExpenditureGoodId);

            builder.Property(a => a.SizeName).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(10);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
