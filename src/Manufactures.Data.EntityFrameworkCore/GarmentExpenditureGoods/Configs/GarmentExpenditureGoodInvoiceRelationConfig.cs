using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentExpenditureGoods.Configs
{
	public class GarmentExpenditureGoodInvoiceRelationConfig : IEntityTypeConfiguration<GarmentExpenditureGoodInvoiceRelationReadModel>
	{
		public void Configure(EntityTypeBuilder<GarmentExpenditureGoodInvoiceRelationReadModel> builder)
		{
			builder.ToTable("GarmentExpenditureGoodInvoiceRelations");
			builder.HasKey(e => e.Identity);

			builder.Property(a => a.ExpenditureGoodNo).HasMaxLength(25);
			builder.Property(a => a.RONo).HasMaxLength(25);
			builder.Property(a => a.UnitCode).HasMaxLength(25);
			builder.Property(a => a.InvoiceNo).HasMaxLength(100);
			 

			builder.HasIndex(i => i.ExpenditureGoodNo).IsUnique().HasFilter("[Deleted]=(0)");

			builder.ApplyAuditTrail();
			builder.ApplySoftDelete();
		}
	}
}
