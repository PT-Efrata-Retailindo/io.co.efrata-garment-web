using Manufactures.Domain.GarmentPreparings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentPreparings.Config
{
    public class GarmentPreparingItemConfig : IEntityTypeConfiguration<GarmentPreparingItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentPreparingItemReadModel> builder)
        {
            builder.ToTable("GarmentPreparingItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentPreparingIdentity)
               .WithMany(a => a.GarmentPreparingItem)
               .HasForeignKey(a => a.GarmentPreparingId);

            builder.Property(o => o.ProductCode)
               .HasMaxLength(25);
            builder.Property(o => o.ProductName)
               .HasMaxLength(100);
            builder.Property(o => o.UomUnit)
               .HasMaxLength(100);
            builder.Property(o => o.DesignColor)
               .HasMaxLength(2000);
            builder.Property(o => o.FabricType)
               .HasMaxLength(100);
            builder.Property(o => o.ROSource)
               .HasMaxLength(100);
			builder.Property(o => o.UId)
				.HasMaxLength(255);
			builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}