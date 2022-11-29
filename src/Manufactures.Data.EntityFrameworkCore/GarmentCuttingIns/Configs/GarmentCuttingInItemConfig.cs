using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingIns.Configs
{
    public class GarmentCuttingInItemConfig : IEntityTypeConfiguration<GarmentCuttingInItemReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentCuttingInItemReadModel> builder)
        {
            builder.ToTable("GarmentCuttingInItems");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.UENNo).HasMaxLength(100);
            builder.Property(p => p.SewingOutNo).HasMaxLength(50);
			builder.Property(p => p.UId).HasMaxLength(255);
			builder.HasOne(w => w.GarmentCuttingIn)
                .WithMany(h => h.Items)
                .HasForeignKey(f => f.CutInId);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
