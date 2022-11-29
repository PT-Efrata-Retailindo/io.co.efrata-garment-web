using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingDOs.Configs
{
    public class GarmentSewingDOItemConfig : IEntityTypeConfiguration<GarmentSewingDOItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSewingDOItemReadModel> builder)
        {
            builder.ToTable("GarmentSewingDOItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentSewingDOIdentity)
                   .WithMany(a => a.GarmentSewingDOItem)
                   .HasForeignKey(a => a.SewingDOId);

            builder.Property(a => a.ProductCode).HasMaxLength(25);
            builder.Property(a => a.ProductName).HasMaxLength(100);
            builder.Property(a => a.DesignColor).HasMaxLength(2000);
            builder.Property(a => a.SizeName).HasMaxLength(100);
            builder.Property(a => a.UomUnit).HasMaxLength(10);
            builder.Property(a => a.Color).HasMaxLength(1000);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}