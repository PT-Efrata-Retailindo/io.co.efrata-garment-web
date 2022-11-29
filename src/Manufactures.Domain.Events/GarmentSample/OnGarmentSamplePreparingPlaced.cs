using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentSamplePreparingPlaced : IGarmentPreparingEvent
    {
        public OnGarmentSamplePreparingPlaced(Guid garmentPreparingId)
        {
            GarmentPreparingId = garmentPreparingId;
        }
        public Guid GarmentPreparingId { get; }
    }
}
