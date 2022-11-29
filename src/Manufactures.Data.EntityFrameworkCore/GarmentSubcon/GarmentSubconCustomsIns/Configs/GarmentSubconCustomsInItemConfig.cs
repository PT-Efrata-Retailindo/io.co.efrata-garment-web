using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentSubconCustomsIns.Configs
{
    public class GarmentSubconCustomsInItemConfig : IEntityTypeConfiguration<GarmentSubconCustomsInItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCustomsInItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconCustomsInItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.SupplierCode).HasMaxLength(25);
            builder.Property(p => p.SupplierName).HasMaxLength(100);
            builder.Property(p => p.DoNo).HasMaxLength(100);
            builder.HasOne(w => w.GarmentSubconCustomsIn)
                .WithMany(h => h.GarmentSubconCustomsInItem)
                .HasForeignKey(f => f.SubconCustomsInId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
