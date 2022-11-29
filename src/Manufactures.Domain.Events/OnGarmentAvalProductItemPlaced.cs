using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentAvalProductItemPlaced : IGarmentAvalProductItemEvent
    {
        public OnGarmentAvalProductItemPlaced(Guid garmentAvalProductItemId)
        {
            GarmentAvalProductItemId = garmentAvalProductItemId;
        }
        public Guid GarmentAvalProductItemId { get; }
    }
}