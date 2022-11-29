using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingDOs.Configs
{
    public class GarmentSewingDOConfig : IEntityTypeConfiguration<GarmentSewingDOReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSewingDOReadModel> builder)
        {
            builder.ToTable("GarmentSewingDOs");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.SewingDONo).HasMaxLength(25);
            builder.Property(a => a.UnitFromCode).HasMaxLength(25);
            builder.Property(a => a.UnitFromName).HasMaxLength(100);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);

            builder.HasIndex(i => i.SewingDONo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}