using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentDeliveryReturnPlaced : IGarmentDeliveryReturnEvent
    {
        public OnGarmentDeliveryReturnPlaced(Guid garmentDeliveryReturnId)
        {
            GarmentDeliveryReturnId = garmentDeliveryReturnId;
        }
        public Guid GarmentDeliveryReturnId { get; }
    }
}