using Manufactures.Domain.GarmentLoadings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentLoadings.Configs
{
	public class GarmentBalanceLoadingConfig : IEntityTypeConfiguration<GarmentBalanceLoadingReadModel>
	{
		public void Configure(EntityTypeBuilder<GarmentBalanceLoadingReadModel> builder)
		{
			builder.ToTable("GarmentBalanceLoadings");
			builder.HasKey(e => e.Identity);

			builder.Property(a => a.RoJob).HasMaxLength(25);
			builder.Property(a => a.Article).HasMaxLength(100);
			builder.Property(a => a.BuyerCode).HasMaxLength(50);
			builder.Property(a => a.Style).HasMaxLength(50);
			builder.Property(a => a.UomUnit).HasMaxLength(50);

			builder.ApplyAuditTrail();
			builder.ApplySoftDelete();
		}
	}
}
