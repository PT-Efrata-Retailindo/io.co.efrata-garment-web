using Manufactures.Domain.GarmentPreparings.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentPreparings.Config
{
    public class GarmentPreparingConfig : IEntityTypeConfiguration<GarmentPreparingReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentPreparingReadModel> builder)
        {
            builder.ToTable("GarmentPreparings");
            builder.HasKey(e => e.Identity);

            builder.Property(o => o.UENNo)
               .HasMaxLength(100);
            builder.Property(o => o.UnitCode)
               .HasMaxLength(25);
            builder.Property(o => o.UnitName)
               .HasMaxLength(100);
            builder.Property(o => o.RONo)
               .HasMaxLength(100);
            builder.Property(o => o.Article)
               .HasMaxLength(500);
            builder.Property(o => o.BuyerCode)
               .HasMaxLength(100);
            builder.Property(o => o.BuyerName)
               .HasMaxLength(500);
			builder.Property(o => o.UId)
				.HasMaxLength(255);
            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}