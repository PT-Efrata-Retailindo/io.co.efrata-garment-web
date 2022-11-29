using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconShrinkagePanels.Config
{
    public class GarmentServiceSubconShrinkagePanelItemConfig : IEntityTypeConfiguration<GarmentServiceSubconShrinkagePanelItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconShrinkagePanelItemReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconShrinkagePanelItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSubconShrinkagePanelIdentity)
                   .WithMany(a => a.GarmentServiceSubconShrinkagePanelItem)
                   .HasForeignKey(a => a.ServiceSubconShrinkagePanelId);


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
