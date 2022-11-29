using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSubcon
{
    public class OnGarmentSubconCustomsOutPlaced : IGarmentSubconCustomsOutEvent
    {
        public OnGarmentSubconCustomsOutPlaced(Guid garmentSubconCustomsOutId)
        {
            GarmentSubconCustomsOutId = garmentSubconCustomsOutId;
        }

        public Guid GarmentSubconCustomsOutId { get; }
    }
}
