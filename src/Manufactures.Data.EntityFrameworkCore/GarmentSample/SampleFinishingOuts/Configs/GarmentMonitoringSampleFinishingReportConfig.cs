using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishingOuts.Configs
{
    public class GarmentMonitoringSampleFinishingReportConfig : IEntityTypeConfiguration<GarmentMonitoringSampleFinishingReportReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentMonitoringSampleFinishingReportReadModel> builder)
        {
            builder.ToTable("GarmentMonitoringSampleFinishingReportTemplate");
            builder.HasKey(e => e.RoJob);

            builder.Property(a => a.RoJob).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(50);
        }
    }
}