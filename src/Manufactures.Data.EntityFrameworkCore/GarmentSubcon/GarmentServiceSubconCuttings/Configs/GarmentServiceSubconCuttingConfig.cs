using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconCuttings.Configs
{
    public class GarmentServiceSubconCuttingConfig : IEntityTypeConfiguration<GarmentServiceSubconCuttingReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconCuttingReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconCuttings");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.SubconNo).HasMaxLength(25);
            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);
            builder.HasIndex(i => i.SubconNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}