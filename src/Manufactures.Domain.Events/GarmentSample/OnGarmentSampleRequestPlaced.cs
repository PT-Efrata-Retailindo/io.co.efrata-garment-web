using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentSampleRequestPlaced : IGarmentSampleRequestEvent
    {
        public OnGarmentSampleRequestPlaced(Guid identity)
        {
            OnGarmentSampleRequestId = identity;
        }
        public Guid OnGarmentSampleRequestId { get; }
    }
}
