using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample.SampleAvalProducts
{
    public class OnGarmentSampleAvalProductItemPlaced : IGarmentSampleAvalProductItemEvent
    {
        public OnGarmentSampleAvalProductItemPlaced(Guid garmentSampleAvalProductItemId)
        {
            GarmentSampleAvalProductItemId = garmentSampleAvalProductItemId;
        }
        public Guid GarmentSampleAvalProductItemId { get; }
    }
}
