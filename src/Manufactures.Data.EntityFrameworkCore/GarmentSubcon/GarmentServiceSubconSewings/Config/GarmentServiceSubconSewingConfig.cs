using System;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconSewings.Config
{
    public class GarmentServiceSubconSewingConfig : IEntityTypeConfiguration<GarmentServiceSubconSewingReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentServiceSubconSewingReadModel> builder)
        {
            builder.ToTable("GarmentServiceSubconSewings");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.ServiceSubconSewingNo).HasMaxLength(25);

            builder.HasIndex(i => i.ServiceSubconSewingNo).IsUnique().HasFilter("[Deleted]=(0)");
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
