using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentAvalComponents.Configs
{
    public class GarmentAvalComponentItemConfig : IEntityTypeConfiguration<GarmentAvalComponentItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentAvalComponentItemReadModel> builder)
        {
            builder.ToTable("GarmentAvalComponentItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.ProductCode).HasMaxLength(25);
            builder.Property(p => p.ProductName).HasMaxLength(100);
            builder.Property(p => p.DesignColor).HasMaxLength(2000);
            builder.Property(p => p.Color).HasMaxLength(1000);
            builder.Property(p => p.SizeName).HasMaxLength(100);
			builder.Property(p => p.UId).HasMaxLength(255);

			builder.HasOne(w => w.GarmentAvalComponent)
                .WithMany(h => h.Items)
                .HasForeignKey(f => f.AvalComponentId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
