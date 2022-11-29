using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentBalanceCuttingPlaced : IGarmentBalanceCuttingEvent
    {
        public OnGarmentBalanceCuttingPlaced(Guid garmentId)
        {
            BalanceCuttingId = garmentId;
        }
        public Guid BalanceCuttingId { get; }
    }
}
