using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.SubconDeliveryLetterOuts.Cofigs
{
    public class GarmentSubconDeliveryLetterOutConfig : IEntityTypeConfiguration<GarmentSubconDeliveryLetterOutReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconDeliveryLetterOutReadModel> builder)
        {
            builder.ToTable("GarmentSubconDeliveryLetterOuts");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.EPONo).HasMaxLength(50);
            builder.Property(p => p.DLNo).HasMaxLength(25);
            builder.Property(p => p.PONo).HasMaxLength(25);
            builder.Property(p => p.UENNo).HasMaxLength(25);
            builder.Property(p => p.ContractType).HasMaxLength(25);
            builder.Property(p => p.Remark).HasMaxLength(4000);
            builder.HasIndex(i => i.DLNo).IsUnique().HasFilter("[Deleted]=(0)");
            builder.Property(p => p.ServiceType).HasMaxLength(50);
            builder.Property(p => p.SubconCategory).HasMaxLength(50);
            builder.Property(p => p.UomUnit).HasMaxLength(10);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
