using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentScrapTransactions.Config
{
	public class GarmentScrapTransactionItemConfig : IEntityTypeConfiguration<GarmentScrapTransactionItemReadModel>
	{
		public void Configure(EntityTypeBuilder<GarmentScrapTransactionItemReadModel> builder)
		{
			builder.ToTable("GarmentScrapTransactionItems");
			builder.HasKey(e => e.Identity);
			builder.HasOne(a => a.GarmentScrapTransactionIdentity)
				   .WithMany(a => a.GarmentScrapTransactionItem)
				   .HasForeignKey(a => a.ScrapTransactionId);
			builder.Property(a => a.UomUnit)
			  .HasMaxLength(25);
			builder.Property(a => a.Description)
			  .HasMaxLength(100);
			builder.Property(a => a.ScrapClassificationName)
			  .HasMaxLength(100);
			builder.ApplyAuditTrail();
			builder.ApplySoftDelete();
		}
	}
}
