using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentAdjustmentPlaced : IGarmentAdjustmentEvent
    {
        public OnGarmentAdjustmentPlaced(Guid identity)
        {
            OnGarmentAdjustmentId = identity;
        }
        public Guid OnGarmentAdjustmentId { get; }
    }
}