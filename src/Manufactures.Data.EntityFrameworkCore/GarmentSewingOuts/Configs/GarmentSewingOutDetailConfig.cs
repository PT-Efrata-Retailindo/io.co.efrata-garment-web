using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingOuts.Configs
{
    public class GarmentSewingOutDetailConfig : IEntityTypeConfiguration<GarmentSewingOutDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSewingOutDetailReadModel> builder)
        {
            builder.ToTable("GarmentSewingOutDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSewingOutItemIdentity)
               .WithMany(a => a.GarmentSewingOutDetail)
               .HasForeignKey(a => a.SewingOutItemId);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);
            builder.Property(a => a.UomUnit)
               .HasMaxLength(25);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
