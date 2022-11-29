using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels
{
    public class GarmentSubconCuttingRelationReadModel : ReadModelBase
    {
        public GarmentSubconCuttingRelationReadModel(Guid identity) : base(identity)
        {
        }

        public Guid GarmentSubconCuttingId { get; internal set; }
        public Guid GarmentCuttingOutDetailId { get; internal set; }
    }
}
