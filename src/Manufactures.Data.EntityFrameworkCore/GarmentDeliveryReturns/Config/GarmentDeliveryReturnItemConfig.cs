using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentDeliveryReturns.Config
{
    public class GarmentDeliveryReturnItemConfig : IEntityTypeConfiguration<GarmentDeliveryReturnItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentDeliveryReturnItemReadModel> builder)
        {
            builder.ToTable("GarmentDeliveryReturnItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentDeliveryReturnIdentity)
               .WithMany(a => a.GarmentDeliveryReturnItem)
               .HasForeignKey(a => a.DRId);

            builder.Property(a => a.ProductCode)
               .HasMaxLength(25);
            builder.Property(a => a.ProductName)
               .HasMaxLength(100);
            builder.Property(a => a.DesignColor)
               .HasMaxLength(2000);
            builder.Property(a => a.RONo)
               .HasMaxLength(100);
            builder.Property(a => a.UomUnit)
               .HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}