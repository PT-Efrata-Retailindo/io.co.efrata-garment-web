using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingAdjustments.ReadModels
{
    public class GarmentCuttingAdjustmentItemReadModel : ReadModelBase
    {
        public GarmentCuttingAdjustmentItemReadModel(Guid identity) : base(identity)
        {
        }
        public Guid AdjustmentCuttingId { get; internal set; }
        public Guid CutInDetailId { get; internal set; }
        public Guid PreparingItemId { get; internal set; }
        public decimal FC { get; internal set; }
        public decimal ActualFC { get; internal set; }
        public decimal Quantity { get; internal set; }
        public decimal ActualQuantity { get; internal set; }
        public virtual GarmentCuttingAdjustmentReadModel GarmentAdjustmentCutting { get; internal set; }
    }
}
