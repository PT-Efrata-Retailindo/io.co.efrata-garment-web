using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample.SampleAvalProducts
{
    public class OnGarmentSampleAvalProductPlaced : IGarmentSampleAvalProductEvent
    {
        public OnGarmentSampleAvalProductPlaced(Guid garmentSampleAvalProductId)
        {
            GarmentSampleAvalProductId = garmentSampleAvalProductId;
        }
        public Guid GarmentSampleAvalProductId { get; }
    }
}
