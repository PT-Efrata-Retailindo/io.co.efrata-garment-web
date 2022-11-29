using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentSampleCuttingInPlaced : IGarmentSampleCuttingInEvent
    {
        public OnGarmentSampleCuttingInPlaced(Guid identity)
        {
            OnGarmentSampleCuttingInId = identity;
        }
        public Guid OnGarmentSampleCuttingInId { get; }
    }
}
