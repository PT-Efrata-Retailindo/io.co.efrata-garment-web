using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.InvoicePackingList.Configs
{
    public class SubconInvoicePackingListItemConfig : IEntityTypeConfiguration<SubconInvoicePackingListItemReadModel>
    {
        public void Configure(EntityTypeBuilder<SubconInvoicePackingListItemReadModel> builder)
        {
            builder.ToTable("SubconInvoicePackingListItems");
            builder.HasKey(e => e.Identity);
            
            builder.Property(p => p.DLNo).HasMaxLength(50);
            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.ProductRemark).HasMaxLength(255);

            builder.Property(p => p.DesignColor).HasMaxLength(255);
            builder.Property(p => p.UomUnit).HasMaxLength(50);

            builder.HasOne(w => w.SubconInvoicePacking)
                .WithMany(m => m.SubconInvoicePackingListItem)
                .HasForeignKey(s => s.InvoicePackingListId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();


        }
    }
}
