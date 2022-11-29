using Manufactures.Domain.GarmentScrapClassifications.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentScrapClassifications.Config
{
	public class GarmentScrapClassificationConfig : IEntityTypeConfiguration<GarmentScrapClassificationReadModel>
	{
		public void Configure(EntityTypeBuilder<GarmentScrapClassificationReadModel> builder)
		{
			builder.ToTable("GarmentScrapClassifications");
			builder.HasKey(e => e.Identity);

			builder.Property(o => o.Code)
			   .HasMaxLength(25);
			builder.Property(o => o.Name)
			   .HasMaxLength(100);
			builder.Property(o => o.Description )
			   .HasMaxLength(500);
			builder.Property(o => o.UId)
			   .HasMaxLength(10);

			builder.ApplyAuditTrail();
			builder.ApplySoftDelete();
		}

	}
}
