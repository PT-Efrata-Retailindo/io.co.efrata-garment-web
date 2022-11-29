using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample.SampleDeliveryReturns
{
    public class OnGarmentSampleDeliveryReturnItemPlaced : IGarmentSampleDeliveryReturnItemEvent
    {
        public OnGarmentSampleDeliveryReturnItemPlaced(Guid garmentSampleDeliveryReturnItemId)
        {
            GarmentSampleDeliveryReturnItemId = garmentSampleDeliveryReturnItemId;
        }
        public Guid GarmentSampleDeliveryReturnItemId { get; }
    }
}
