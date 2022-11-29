using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconFabricWashes.Config
{
    public class GarmentServiceSubconFabricWashItemConfig : IEntityTypeConfiguration<GarmentServiceSubconFabricWashItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconFabricWashItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconFabricWashItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSubconFabricWashIdentity)
                   .WithMany(a => a.GarmentServiceSubconFabricWashItem)
                   .HasForeignKey(a => a.ServiceSubconFabricWashId);


            builder.Property(a => a.UnitExpenditureNo).HasMaxLength(25);
            builder.Property(a => a.UnitSenderCode).HasMaxLength(25);
            builder.Property(a => a.UnitSenderName).HasMaxLength(100);
            builder.Property(a => a.UnitRequestCode).HasMaxLength(25);
            builder.Property(a => a.UnitRequestName).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
