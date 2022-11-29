using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentSampleFinishingOutPlaced : IGarmentSampleFinishingOutEvent
    {
        public OnGarmentSampleFinishingOutPlaced(Guid identity)
        {
            OnGarmentSampleFinishingOutId = identity;
        }
        public Guid OnGarmentSampleFinishingOutId { get; }
    }
}
