using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentAvalComponents.Configs
{
    public class GarmentAvalComponentConfig : IEntityTypeConfiguration<GarmentAvalComponentReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentAvalComponentReadModel> builder)
        {
            builder.ToTable("GarmentAvalComponents");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.AvalComponentNo).HasMaxLength(25);
            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);
            builder.Property(p => p.AvalComponentType).HasMaxLength(25);
            builder.Property(p => p.RONo).HasMaxLength(25);
            builder.Property(p => p.Article).HasMaxLength(50);
            builder.Property(p => p.ComodityCode).HasMaxLength(25);
            builder.Property(p => p.ComodityName).HasMaxLength(100);

            builder.HasIndex(i => i.AvalComponentNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
