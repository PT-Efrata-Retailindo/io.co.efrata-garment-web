using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentScrapTransactions.Config
{
	public class GarmentScrapTransactionConfig : IEntityTypeConfiguration<GarmentScrapTransactionReadModel>
	{
		public void Configure(EntityTypeBuilder<GarmentScrapTransactionReadModel> builder)
		{
			builder.ToTable("GarmentScrapTransactions");
			builder.HasKey(e => e.Identity);

			builder.Property(o => o.TransactionNo)
			   .HasMaxLength(25);
			builder.Property(o => o.TransactionType)
			   .HasMaxLength(25);
			builder.Property(o => o.ScrapDestinationName)
			   .HasMaxLength(100);
			builder.Property(o => o.ScrapSourceName)
			   .HasMaxLength(100);

			builder.ApplyAuditTrail();
			builder.ApplySoftDelete();
		}

	}
}
