using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentCuttingAdjustments.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingAdjustments
{
    public class GarmentCuttingAdjustmentItem : AggregateRoot<GarmentCuttingAdjustmentItem, GarmentCuttingAdjustmentItemReadModel>
    {
        public Guid AdjustmentCuttingId { get; private set; }
        public Guid CutInDetailId { get; private set; }
        public Guid PreparingItemId { get; private set; }
        public decimal FC { get; private set; }
        public decimal ActualFC { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal ActualQuantity { get; private set; }

        public GarmentCuttingAdjustmentItem(Guid identity, Guid adjustmentCuttingId, Guid cutInDetailId, Guid preparingItemId, decimal fC, decimal actualFC, decimal quantity, decimal actualQuantity) : base(identity)
        {
            Identity = identity;
            AdjustmentCuttingId = adjustmentCuttingId;
            CutInDetailId = cutInDetailId;
            PreparingItemId = preparingItemId;
            FC = fC;
            ActualFC = actualFC;
            Quantity = quantity;
            ActualQuantity = actualQuantity;

            ReadModel = new GarmentCuttingAdjustmentItemReadModel(Identity)
            {
                AdjustmentCuttingId = AdjustmentCuttingId,
                PreparingItemId = PreparingItemId,
                CutInDetailId = CutInDetailId,
                FC = FC,
                ActualFC = ActualFC,
                Quantity = Quantity,
                ActualQuantity = ActualQuantity
            };

            ReadModel.AddDomainEvent(new OnGarmentCuttingAdjustmentPlaced(Identity));
        }

        public GarmentCuttingAdjustmentItem(GarmentCuttingAdjustmentItemReadModel readModel) : base(readModel)
        {
            AdjustmentCuttingId = readModel.AdjustmentCuttingId;
            PreparingItemId = readModel.PreparingItemId;
            CutInDetailId = readModel.CutInDetailId;
            FC = readModel.FC;
            ActualFC = readModel.ActualFC;
            Quantity = readModel.Quantity;
            ActualQuantity = readModel.ActualQuantity;
        }
        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentCuttingAdjustmentItem GetEntity()
        {
            return this;
        }

    }
}
