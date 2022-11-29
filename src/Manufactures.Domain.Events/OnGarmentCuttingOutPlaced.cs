using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentCuttingOutPlaced : IGarmentCuttingOutEvent
    {
        public OnGarmentCuttingOutPlaced(Guid identity)
        {
            GarmentCuttingOutId = identity;
        }
        public Guid GarmentCuttingOutId { get; }
    }
}
