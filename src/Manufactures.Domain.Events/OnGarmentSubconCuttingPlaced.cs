using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentSubconCuttingPlaced : IGarmentSubconCuttingEvent
    {
        public OnGarmentSubconCuttingPlaced(Guid identity)
        {
            GarmentSubconCuttingId = identity;
        }
        public Guid GarmentSubconCuttingId { get; }
    }

}
