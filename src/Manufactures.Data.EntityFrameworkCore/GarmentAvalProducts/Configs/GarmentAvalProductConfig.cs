using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Manufactures.Data.EntityFrameworkCore.GarmentAvalProducts.Configs
{
    public class GarmentAvalProductConfig : IEntityTypeConfiguration<GarmentAvalProductReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentAvalProductReadModel> builder)
        {
            builder.ToTable("GarmentAvalProducts");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.RONo).HasMaxLength(100);
            builder.Property(a => a.Article).HasMaxLength(100);

            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}