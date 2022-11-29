using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSubcon
{
    public class OnGarmentServiceSubconFabricWashPlaced : IGarmentServiceSubconFabricWashEvent
    {
        public OnGarmentServiceSubconFabricWashPlaced(Guid garmentServiceSubconFabricWashId)
        {
            GarmentServiceSubconFabricWashId = garmentServiceSubconFabricWashId;
        }

        public Guid GarmentServiceSubconFabricWashId { get; }
    }
}
