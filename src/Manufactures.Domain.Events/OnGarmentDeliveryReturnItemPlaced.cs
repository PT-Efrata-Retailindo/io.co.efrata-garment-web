using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentDeliveryReturnItemPlaced : IGarmentDeliveryReturnItemEvent
    {
        public OnGarmentDeliveryReturnItemPlaced(Guid garmentDeliveryReturnItemId)
        {
            GarmentDeliveryReturnItemId = garmentDeliveryReturnItemId;
        }
        public Guid GarmentDeliveryReturnItemId { get; }
    }
}