using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentSubconCuttingOutPlaced : IGarmentSubconCuttingOutEvent
    {
        public OnGarmentSubconCuttingOutPlaced(Guid identity)
        {
            GarmentCuttingOutId = identity;
        }
        public Guid GarmentCuttingOutId { get; }
    }
}
