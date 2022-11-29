using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconShrinkagePanels.Config
{
    public class GarmentServiceSubconShrinkagePanelConfig : IEntityTypeConfiguration<GarmentServiceSubconShrinkagePanelReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconShrinkagePanelReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconShrinkagePanels");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.ServiceSubconShrinkagePanelNo).HasMaxLength(25);
            builder.Property(a => a.Remark).HasMaxLength(1000);

            builder.HasIndex(i => i.ServiceSubconShrinkagePanelNo).IsUnique().HasFilter("[Deleted]=(0)");
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
