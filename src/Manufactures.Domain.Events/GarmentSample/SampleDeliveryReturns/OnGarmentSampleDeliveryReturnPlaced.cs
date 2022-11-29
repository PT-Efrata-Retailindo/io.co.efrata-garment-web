using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample.SampleDeliveryReturns
{
    public class OnGarmentSampleDeliveryReturnPlaced : IGarmentSampleDeliveryReturnEvent
    {
        public OnGarmentSampleDeliveryReturnPlaced(Guid garmentSampleDeliveryReturnId)
        {
            GarmentSampleDeliveryReturnId = garmentSampleDeliveryReturnId;
        }
        public Guid GarmentSampleDeliveryReturnId { get; }
    }
}
