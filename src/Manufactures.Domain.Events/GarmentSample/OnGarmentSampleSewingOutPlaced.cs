using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentSampleSewingOutPlaced : IGarmentSampleSewingOutEvent
    {
        public OnGarmentSampleSewingOutPlaced(Guid garmentSampleSewingOutId)
        {
            GarmentSampleSewingOutId = garmentSampleSewingOutId;
        }
        public Guid GarmentSampleSewingOutId { get; }
    }
}
