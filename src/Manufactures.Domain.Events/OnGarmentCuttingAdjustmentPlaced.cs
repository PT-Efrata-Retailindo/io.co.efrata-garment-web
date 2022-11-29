using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentCuttingAdjustmentPlaced : IGarmentCuttingAdjustmentEvent
    {
        public OnGarmentCuttingAdjustmentPlaced(Guid identity)
        {
            GarmentCuttingAdjustmentId = identity;
        }
        public Guid GarmentCuttingAdjustmentId { get; }
    }
}