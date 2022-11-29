using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentAvalProducts.Configs
{
    public class GarmentAvalProductItemConfig : IEntityTypeConfiguration<GarmentAvalProductItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentAvalProductItemReadModel> builder)
        {
            builder.ToTable("GarmentAvalProductItems");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentAvalProductIdentity)
               .WithMany(a => a.GarmentAvalProductItem)
               .HasForeignKey(a => a.APId);

            builder.Property(o => o.ProductCode).HasMaxLength(25);
            builder.Property(o => o.ProductName).HasMaxLength(100);
            builder.Property(o => o.DesignColor).HasMaxLength(2000);
            builder.Property(o => o.UomUnit).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();

        }
    }
}