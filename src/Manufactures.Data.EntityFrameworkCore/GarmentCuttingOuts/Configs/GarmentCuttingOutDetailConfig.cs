using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingOuts.Configs
{
    public class GarmentCuttingOutDetailConfig : IEntityTypeConfiguration<GarmentCuttingOutDetailReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentCuttingOutDetailReadModel> builder)
        {
            builder.ToTable("GarmentCuttingOutDetails");
            builder.HasKey(e => e.Identity);
            builder.HasOne(a => a.GarmentCuttingOutItemIdentity)
               .WithMany(a => a.GarmentCuttingOutDetail)
               .HasForeignKey(a => a.CutOutItemId);

            builder.Property(a => a.SizeName)
               .HasMaxLength(100);
            builder.Property(a => a.Color)
               .HasMaxLength(1000);
            builder.Property(a => a.CuttingOutUomUnit)
               .HasMaxLength(10);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}