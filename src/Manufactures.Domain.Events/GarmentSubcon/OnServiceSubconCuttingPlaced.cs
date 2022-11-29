using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSubcon
{
    public class OnServiceSubconCuttingPlaced : IServiceSubconCuttingEvent
    {
        public OnServiceSubconCuttingPlaced(Guid identity)
        {
            OnServiceSubconCuttingId = identity;
        }
        public Guid OnServiceSubconCuttingId { get; }
    }
}
