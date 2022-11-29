using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingOuts.Configs
{
    public class GarmentCuttingOutItemConfig : IEntityTypeConfiguration<GarmentCuttingOutItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentCuttingOutItemReadModel> builder)
        {
            builder.ToTable("GarmentCuttingOutItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentCuttingOutIdentity)
                   .WithMany(a => a.GarmentCuttingOutItem)
                   .HasForeignKey(a => a.CutOutId);

            builder.Property(a => a.ProductCode)
               .HasMaxLength(25);
            builder.Property(a => a.ProductName)
               .HasMaxLength(100);
            builder.Property(a => a.DesignColor)
               .HasMaxLength(2000);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}