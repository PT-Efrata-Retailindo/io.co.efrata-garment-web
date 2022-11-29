using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentSubconContracts.Configs
{
    public class GarmentSubconContractItemConfig : IEntityTypeConfiguration<GarmentSubconContractItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconContractItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconContractItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.UomUnit).HasMaxLength(50);
            builder.HasOne(w => w.GarmentSubconContract)
                .WithMany(h => h.GarmentSubconContractItem)
                .HasForeignKey(f => f.SubconContractId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
