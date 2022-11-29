using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentSampleStockPlaced : IGarmentSampleStockEvent
    {
        public OnGarmentSampleStockPlaced(Guid garmentSampleStockId)
        {
            GarmentSampleStockId = garmentSampleStockId;
        }
        public Guid GarmentSampleStockId { get; }
    }
}
