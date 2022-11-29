using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingIns.Configs
{
    public class SewingInHomeListViewConfig : IEntityTypeConfiguration<SewingInHomeListViewReadModel>
    {
        //Enhance Jason Aug 2021
        public void Configure(EntityTypeBuilder<SewingInHomeListViewReadModel> builder)
        {
            builder.ToTable("SewingInHomeListView");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.SewingInNo).HasMaxLength(25);
            builder.Property(a => a.Article).HasMaxLength(50);          
            builder.Property(a => a.RONo).HasMaxLength(25);
            builder.Property(a => a.UnitCode).HasMaxLength(25);
            builder.Property(a => a.SewingFrom).HasMaxLength(25);
            builder.Property(a => a.UnitFromCode).HasMaxLength(25);
        }
    }
}