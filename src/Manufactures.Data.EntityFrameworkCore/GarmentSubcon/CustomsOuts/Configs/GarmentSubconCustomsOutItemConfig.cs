using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.CustomsOuts.Configs
{
    public class GarmentSubconCustomsOutItemConfig : IEntityTypeConfiguration<GarmentSubconCustomsOutItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCustomsOutItemReadModel> builder)
        {
            builder.ToTable("GarmentSubconCustomsOutItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.SubconDLOutNo).HasMaxLength(25);
            builder.HasOne(w => w.GarmentSubconCustomsOut)
                .WithMany(h => h.GarmentSubconCustomsOutItem)
                .HasForeignKey(f => f.SubconCustomsOutId);
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
