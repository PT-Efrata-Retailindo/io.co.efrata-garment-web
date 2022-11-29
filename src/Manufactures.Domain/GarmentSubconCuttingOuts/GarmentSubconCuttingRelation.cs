using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using System;

namespace Manufactures.Domain.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingRelation : AggregateRoot<GarmentSubconCuttingRelation, GarmentSubconCuttingRelationReadModel>
    {
        public Guid GarmentSubconCuttingId { get; private set; }
        public Guid GarmentCuttingOutDetailId { get; private set; }

        public GarmentSubconCuttingRelation(Guid identity, Guid garmentSubconCuttingId, Guid garmentCuttingOutId) : base(identity)
        {
            GarmentSubconCuttingId = garmentSubconCuttingId;
            GarmentCuttingOutDetailId = garmentCuttingOutId;

            ReadModel = new GarmentSubconCuttingRelationReadModel(Identity)
            {
                GarmentSubconCuttingId = garmentSubconCuttingId,
                GarmentCuttingOutDetailId = garmentCuttingOutId,
            };

            ReadModel.AddDomainEvent(new OnGarmentSubconCuttingRelationPlaced(Identity));
        }

        public GarmentSubconCuttingRelation(GarmentSubconCuttingRelationReadModel readModel) : base(readModel)
        {
            GarmentSubconCuttingId = readModel.GarmentSubconCuttingId;
            GarmentCuttingOutDetailId = readModel.GarmentCuttingOutDetailId;
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSubconCuttingRelation GetEntity()
        {
            return this;
        }
    }
}
