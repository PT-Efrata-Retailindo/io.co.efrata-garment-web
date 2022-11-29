using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentSubconCustomsIns.Configs
{
    public class GarmentSubconCustomsInConfig : IEntityTypeConfiguration<GarmentSubconCustomsInReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCustomsInReadModel> builder)
        {
            builder.ToTable("GarmentSubconCustomsIns");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.BcNo).HasMaxLength(255);
            builder.Property(p => p.BcType).HasMaxLength(255);
            builder.Property(p => p.SubconType).HasMaxLength(255);
            builder.Property(p => p.SubconContractNo).HasMaxLength(100);
            builder.Property(p => p.SupplierCode).HasMaxLength(25);
            builder.Property(p => p.SupplierName).HasMaxLength(100);
            builder.Property(p => p.Remark).HasMaxLength(1000);
            builder.HasIndex(i => i.BcNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
