using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentFinishedGoodStockHistoryPlaced : IGarmentFinishedGoodStockHistoryEvent
    {
        public OnGarmentFinishedGoodStockHistoryPlaced(Guid garmentFinishedGoodStockHistoryId)
        {
            GarmentFinishedGoodStockHistoryId = garmentFinishedGoodStockHistoryId;
        }
        public Guid GarmentFinishedGoodStockHistoryId { get; }
    }
}