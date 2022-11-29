using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSubcon
{
    public class OnGarmentSubconDeliveryLetterOutPlaced : IGarmentSubconDeliveryLetterOutEvent
    {
        public OnGarmentSubconDeliveryLetterOutPlaced(Guid garmentSubconDeliveryLetterOutId)
        {
            GarmentSubconDeliveryLetterOutId = garmentSubconDeliveryLetterOutId;
        }

        public Guid GarmentSubconDeliveryLetterOutId { get; }
    }
}
