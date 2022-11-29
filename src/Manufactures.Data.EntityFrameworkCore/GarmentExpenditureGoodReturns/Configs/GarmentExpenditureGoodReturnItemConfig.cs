using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentExpenditureGoodReturns.Configs
{
    public class GarmentExpenditureGoodReturnItemConfig : IEntityTypeConfiguration<GarmentExpenditureGoodReturnItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentExpenditureGoodReturnItemReadModel> builder)
        {
            builder.ToTable("GarmentExpenditureGoodReturnItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentExpenditureGoodReturn)
                   .WithMany(a => a.Items)
                   .HasForeignKey(a => a.ReturId);

            builder.Property(a => a.SizeName).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(10);
            builder.Property(a => a.Description).HasMaxLength(2000);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
