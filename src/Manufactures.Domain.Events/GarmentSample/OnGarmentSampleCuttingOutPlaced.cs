using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentSampleCuttingOutPlaced : IGarmentSampleCuttingOutEvent
    {
        public OnGarmentSampleCuttingOutPlaced(Guid identity)
        {
            OnGarmentSampleCuttingOutId = identity;
        }
        public Guid OnGarmentSampleCuttingOutId { get; }
    }
}
