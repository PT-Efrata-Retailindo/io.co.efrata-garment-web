using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentFinishingOutPlaced : IGarmentFinishingOutEvent
    {
        public OnGarmentFinishingOutPlaced(Guid garmentFinishingOutId)
        {
            GarmentFinishingOutId = garmentFinishingOutId;
        }
        public Guid GarmentFinishingOutId { get; }
    }
}
