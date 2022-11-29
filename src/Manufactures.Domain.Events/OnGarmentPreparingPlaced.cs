using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentPreparingPlaced : IGarmentPreparingEvent
    {
        public OnGarmentPreparingPlaced(Guid garmentPreparingId)
        {
            GarmentPreparingId = garmentPreparingId;
        }
        public Guid GarmentPreparingId { get; }
    }
}