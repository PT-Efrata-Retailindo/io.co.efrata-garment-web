using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleRequests.Configs
{
    public class GarmentSampleRequestConfig : IEntityTypeConfiguration<GarmentSampleRequestReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSampleRequestReadModel> builder)
        {
            builder.ToTable("GarmentSampleRequests");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.Attached).HasMaxLength(255);
            builder.Property(p => p.SampleCategory).HasMaxLength(50);
            builder.Property(p => p.ComodityCode).HasMaxLength(25);
            builder.Property(p => p.ComodityName).HasMaxLength(255);
            builder.Property(p => p.Remark).HasMaxLength(4000);
            builder.Property(a => a.BuyerName).HasMaxLength(255);
            builder.Property(p => p.BuyerCode).HasMaxLength(25);
            builder.Property(p => p.Packing).HasMaxLength(255);
            builder.Property(p => p.SampleRequestNo).HasMaxLength(30);
            builder.Property(p => p.RONoCC).HasMaxLength(15);
            builder.Property(p => p.RONoSample).HasMaxLength(15);
            builder.Property(p => p.POBuyer).HasMaxLength(255);
            builder.Property(p => p.Remark).HasMaxLength(4000);
            builder.Property(p => p.SampleType).HasMaxLength(255);
            builder.Property(p => p.ReceivedBy).HasMaxLength(255);
            builder.Property(p => p.RejectedBy).HasMaxLength(255);
            builder.Property(p => p.RevisedBy).HasMaxLength(255);
            builder.Property(p => p.RevisedReason).HasMaxLength(1000);
            builder.Property(p => p.ImagesPath).HasMaxLength(1000);
            builder.Property(p => p.DocumentsPath).HasMaxLength(1000);
            builder.Property(p => p.DocumentsFileName).HasMaxLength(255);
            builder.Property(p => p.ImagesName).HasMaxLength(255);
            builder.Property(p => p.SectionCode).HasMaxLength(25);
            builder.Property(p => p.SampleTo).HasMaxLength(50);
            builder.HasIndex(i => i.RONoSample).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
