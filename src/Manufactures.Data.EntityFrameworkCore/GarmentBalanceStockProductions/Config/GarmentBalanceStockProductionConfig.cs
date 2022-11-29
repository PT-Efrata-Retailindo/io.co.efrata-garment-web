//using Manufactures.Domain.GarmentBalanceStockProductions.ReadModels;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Manufactures.Data.EntityFrameworkCore.GarmentBalanceStockProductions.Config
//{
//	public class GarmentBalanceStockProductionConfig : IEntityTypeConfiguration<GarmentBalanceStockProductionReadModel>
//	{
//		public void Configure(EntityTypeBuilder<GarmentBalanceStockProductionReadModel> builder)
//		{
//			builder.ToTable("GarmentBalanceStockProductions");
//			builder.HasKey(e => e.Identity);
//			builder.Property(a => a.RO)
//			  .HasMaxLength(25);
//			builder.Property(a => a.ArticleNo)
//			  .HasMaxLength(100);
//			builder.Property(a => a.ComodityName)
//			  .HasMaxLength(100);
//			builder.ApplyAuditTrail();
//			builder.ApplySoftDelete();
//		}
//	}
//}
