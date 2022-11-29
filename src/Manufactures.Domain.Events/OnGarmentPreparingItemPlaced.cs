using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentPreparingItemPlaced : IGarmentPreparingItemEvent
    {
        public OnGarmentPreparingItemPlaced(Guid garmentPreparingItemId)
        {
            GarmentPreparingItemId = garmentPreparingItemId;
        }
        public Guid GarmentPreparingItemId { get; }
    }
}