using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentSampleFinishedGoodStockPlaced : IGarmentSampleFinishedGoodStockEvent
    {
        public OnGarmentSampleFinishedGoodStockPlaced(Guid garmentSampleFinishedGoodId)
        {
            GarmentSampleFinishedGoodId = garmentSampleFinishedGoodId;
        }
        public Guid GarmentSampleFinishedGoodId { get; }
    }
}

