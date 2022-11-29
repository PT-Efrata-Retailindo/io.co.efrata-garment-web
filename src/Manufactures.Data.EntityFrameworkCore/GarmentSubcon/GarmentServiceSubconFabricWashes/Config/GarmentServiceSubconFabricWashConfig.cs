using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconFabricWashes.Config
{
    public class GarmentServiceSubconFabricWashConfig : IEntityTypeConfiguration<GarmentServiceSubconFabricWashReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconFabricWashReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconFabricWashes");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.ServiceSubconFabricWashNo).HasMaxLength(25);

            builder.HasIndex(i => i.ServiceSubconFabricWashNo).IsUnique().HasFilter("[Deleted]=(0)");
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
