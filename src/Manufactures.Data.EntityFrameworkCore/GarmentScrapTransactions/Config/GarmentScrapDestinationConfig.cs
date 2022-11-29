using Manufactures.Domain.GarmentScrapDestinations.ReadModels;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentScrapTransactions.Config
{
	public class GarmentScrapDestinationConfig : IEntityTypeConfiguration<GarmentScrapDestinationReadModel>
	{
		public void Configure(EntityTypeBuilder<GarmentScrapDestinationReadModel> builder)
		{
			builder.ToTable("GarmentScrapDestinations");
			builder.HasKey(e => e.Identity);
			builder.Property(a => a.Code)
			  .HasMaxLength(25);
			builder.Property(a => a.Name)
			  .HasMaxLength(50);
			builder.Property(a => a.UId)
			  .HasMaxLength(10);
			builder.Property(a => a.Description )
			  .HasMaxLength(100);
			builder.ApplyAuditTrail();
			builder.ApplySoftDelete();
		}
	}
}
