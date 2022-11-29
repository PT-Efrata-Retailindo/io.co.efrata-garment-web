using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentSampleFinishingInPlaced : IGarmentFinishingInEvent
    {
        public OnGarmentSampleFinishingInPlaced(Guid identity)
        {
            OnGarmentSampleFinishingInId = identity;
        }
        public Guid OnGarmentSampleFinishingInId { get; }
    }
}
