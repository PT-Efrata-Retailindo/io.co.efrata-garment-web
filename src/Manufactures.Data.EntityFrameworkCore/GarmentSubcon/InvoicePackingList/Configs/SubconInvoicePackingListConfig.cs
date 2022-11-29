using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.InvoicePackingList.Configs
{
    public class SubconInvoicePackingListConfig : IEntityTypeConfiguration<SubconInvoicePackingListReadModel>
    {
        public void Configure(EntityTypeBuilder<SubconInvoicePackingListReadModel> builder)
        {
            builder.ToTable("SubconInvoicePackingList");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.InvoiceNo).HasMaxLength(50);
            builder.Property(a => a.BCType).HasMaxLength(25);
            builder.Property(a => a.SupplierCode).HasMaxLength(50);
            builder.Property(a => a.SupplierName).HasMaxLength(50);
            builder.Property(a => a.SupplierAddress).HasMaxLength(255);
            builder.Property(a => a.ContractNo).HasMaxLength(50);

            builder.HasIndex(i => i.InvoiceNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();

        }
    }
}
