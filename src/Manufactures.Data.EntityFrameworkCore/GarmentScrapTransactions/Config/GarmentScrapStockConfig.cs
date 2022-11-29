using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentScrapTransactions.Config
{
	public class GarmentScrapStockConfig : IEntityTypeConfiguration<GarmentScrapStockReadModel>
	{
		public void Configure(EntityTypeBuilder<GarmentScrapStockReadModel> builder)
		{
			builder.ToTable("GarmentScrapStocks");
			builder.HasKey(e => e.Identity);
			builder.Property(a => a.UomUnit)
			  .HasMaxLength(25);
			builder.Property(a => a.ScrapDestinationName)
			  .HasMaxLength(100);
			builder.Property(a => a.ScrapClassificationName)
			  .HasMaxLength(100);
			builder.Property(a => a.UomUnit)
			  .HasMaxLength(25);
			builder.ApplyAuditTrail();
			builder.ApplySoftDelete();
		}
	}
}
