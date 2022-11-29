using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingOuts.Configs
{
    public class GarmentSewingOutConfig : IEntityTypeConfiguration<GarmentSewingOutReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSewingOutReadModel> builder)
        {
            builder.ToTable("GarmentSewingOuts");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.SewingOutNo).HasMaxLength(25);
            builder.Property(a => a.BuyerName).HasMaxLength(100);
            builder.Property(a => a.BuyerCode).HasMaxLength(25);
            builder.Property(a => a.UnitToCode).HasMaxLength(25);
            builder.Property(a => a.UnitToName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.SewingTo).HasMaxLength(100);

            builder.HasIndex(i => i.SewingOutNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
