using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleExpenditureGoods.Configs
{
    public class GarmentSampleExpenditureGoodItemConfig : IEntityTypeConfiguration<GarmentSampleExpenditureGoodItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleExpenditureGoodItemReadModel> builder)
        {
            builder.ToTable("GarmentSampleExpenditureGoodItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSampleExpenditureGood)
                   .WithMany(a => a.Items)
                   .HasForeignKey(a => a.ExpenditureGoodId);

            builder.Property(a => a.SizeName).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(10);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}