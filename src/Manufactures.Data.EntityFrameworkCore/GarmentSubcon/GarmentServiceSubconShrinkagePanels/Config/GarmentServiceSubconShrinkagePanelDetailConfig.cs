using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconShrinkagePanels.Config
{
    public class GarmentServiceSubconShrinkagePanelDetailConfig : IEntityTypeConfiguration<GarmentServiceSubconShrinkagePanelDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconShrinkagePanelDetailReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconShrinkagePanelDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentServiceSubconShrinkagePanelItemIdentity)
                   .WithMany(a => a.GarmentServiceSubconShrinkagePanelDetail)
                   .HasForeignKey(a => a.ServiceSubconShrinkagePanelItemId);

            builder.Property(a => a.ProductCode).HasMaxLength(25);
            builder.Property(a => a.ProductName).HasMaxLength(100);
            builder.Property(a => a.ProductRemark).HasMaxLength(1000);
            builder.Property(a => a.DesignColor).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
