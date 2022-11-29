using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentSubconCuttingRelationPlaced : IGarmentSubconCuttingEvent
    {
        public OnGarmentSubconCuttingRelationPlaced(Guid identity)
        {
            GarmentSubconCuttingRelationId = identity;
        }
        public Guid GarmentSubconCuttingRelationId { get; }
    }

}
