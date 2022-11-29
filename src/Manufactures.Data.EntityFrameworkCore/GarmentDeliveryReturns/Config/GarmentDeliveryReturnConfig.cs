using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Manufactures.Data.EntityFrameworkCore.GarmentDeliveryReturns.Config
{
    public class GarmentDeliveryReturnConfig : IEntityTypeConfiguration<GarmentDeliveryReturnReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentDeliveryReturnReadModel> builder)
        {
            builder.ToTable("GarmentDeliveryReturns");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.DRNo)
               .HasMaxLength(25);
            builder.Property(a => a.RONo)
               .HasMaxLength(100);
            builder.Property(a => a.Article)
               .HasMaxLength(100);
            builder.Property(a => a.UnitDONo)
               .HasMaxLength(100);
            builder.Property(a => a.ReturnType)
               .HasMaxLength(25);
            builder.Property(a => a.UnitCode)
               .HasMaxLength(25);
            builder.Property(a => a.UnitName)
               .HasMaxLength(100);
            builder.Property(a => a.StorageCode)
               .HasMaxLength(25);
            builder.Property(a => a.StorageName)
               .HasMaxLength(100);

            builder.HasIndex(i => i.DRNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}