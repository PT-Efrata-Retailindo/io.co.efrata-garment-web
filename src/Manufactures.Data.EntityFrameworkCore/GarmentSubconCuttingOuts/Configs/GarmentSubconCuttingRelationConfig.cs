using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubconCuttingOuts.Configs
{
    public class GarmentSubconCuttingRelationConfig : IEntityTypeConfiguration<GarmentSubconCuttingRelationReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentSubconCuttingRelationReadModel> builder)
        {
            builder.ToTable("GarmentSubconCuttingRelations");
            builder.HasKey(e => e.Identity);

            builder.HasIndex(i => i.GarmentCuttingOutDetailId).IsUnique().HasFilter("[Deleted]=(0)");
            builder.HasIndex(i => new { i.GarmentCuttingOutDetailId, i.GarmentSubconCuttingId }).IsUnique().HasFilter("[Deleted]=(0)");

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}
