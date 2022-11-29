using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingOuts.Configs
{
    public class GarmentCuttingOutConfig : IEntityTypeConfiguration<GarmentCuttingOutReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentCuttingOutReadModel> builder)
        {
            builder.ToTable("GarmentCuttingOuts");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.CutOutNo).HasMaxLength(25);
            builder.Property(a => a.CuttingOutType).HasMaxLength(25);
            builder.Property(a => a.UnitFromCode).HasMaxLength(25);
            builder.Property(a => a.UnitFromName).HasMaxLength(100);
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.UnitName).HasMaxLength(100);
            builder.Property(a => a.ComodityCode).HasMaxLength(25);
            builder.Property(a => a.ComodityName).HasMaxLength(100);
            builder.Property(a => a.POSerialNumber)
               .HasMaxLength(100);

            builder.HasIndex(i => i.CutOutNo).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
