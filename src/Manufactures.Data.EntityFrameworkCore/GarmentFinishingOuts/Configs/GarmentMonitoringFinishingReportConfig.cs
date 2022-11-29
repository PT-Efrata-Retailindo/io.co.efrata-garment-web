using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishingOuts.Configs
{
	public class GarmentMonitoringFinishingReportConfig : IEntityTypeConfiguration<GarmentMonitoringFinishingReportReadModel>
	{
		//Enhance Jason Aug 2021
		public void Configure(EntityTypeBuilder<GarmentMonitoringFinishingReportReadModel> builder)
		{
			builder.ToTable("GarmentMonitoringFinishingReportTemplate");
			builder.HasKey(e => e.RoJob);

			builder.Property(a => a.RoJob).HasMaxLength(25);
			builder.Property(a => a.Article).HasMaxLength(100);
			builder.Property(a => a.UomUnit).HasMaxLength(50);
		}
	}
}
