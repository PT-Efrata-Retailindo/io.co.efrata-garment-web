using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentAvalProductPlaced : IGarmentAvalProductEvent
    {
        public OnGarmentAvalProductPlaced(Guid garmentAvalProductId)
        {
            GarmentAvalProductId = garmentAvalProductId;
        }
        public Guid GarmentAvalProductId { get; }
    }
}