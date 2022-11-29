using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.CustomsOuts.Configs
{
    public class GarmentSubconCustomsOutConfig : IEntityTypeConfiguration<GarmentSubconCustomsOutReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCustomsOutReadModel> builder)
        {
            builder.ToTable("GarmentSubconCustomsOuts");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.CustomsOutNo).HasMaxLength(25);
            builder.Property(p => p.SubconContractNo).HasMaxLength(25);
            builder.Property(p => p.SubconType).HasMaxLength(50);
            builder.Property(p => p.CustomsOutType).HasMaxLength(50);
            builder.Property(p => p.SupplierCode).HasMaxLength(25);
            builder.Property(p => p.Remark).HasMaxLength(4000);
            builder.Property(a => a.SupplierName).HasMaxLength(100);
            builder.Property(a => a.SupplierCode).HasMaxLength(25);
            builder.Property(p => p.SubconCategory).HasMaxLength(100);
            builder.HasIndex(i => i.CustomsOutNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}

