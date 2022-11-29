using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentFinishedGoodStockPlaced : IGarmentFinishedGoodStockEvent
    {
        public OnGarmentFinishedGoodStockPlaced(Guid garmentFinishedGoodId)
        {
            GarmentFinishedGoodId = garmentFinishedGoodId;
        }
        public Guid GarmentFinishedGoodId { get; }
    }
}