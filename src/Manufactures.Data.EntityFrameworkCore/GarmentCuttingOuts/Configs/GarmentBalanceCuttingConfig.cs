using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingOuts.Configs
{
	public class GarmentBalanceCuttingConfig : IEntityTypeConfiguration<GarmentBalanceCuttingReadModel>
	{
		public void Configure(EntityTypeBuilder<GarmentBalanceCuttingReadModel> builder)
		{
			builder.ToTable("GarmentBalanceCuttings");
			builder.HasKey(e => e.Identity);

			builder.Property(a => a.RoJob).HasMaxLength(25);
			builder.Property(a => a.Article).HasMaxLength(100);
			builder.Property(a => a.BuyerCode).HasMaxLength(50);
			builder.Property(a => a.Style).HasMaxLength(50);
			 

			builder.ApplyAuditTrail();
			builder.ApplySoftDelete();
		}
	}
}
