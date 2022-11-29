using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSubcon.InvoicePackingList
{
    public class OnSubconPackingListPlaced : ISubconPackingListEvent
    {
        public OnSubconPackingListPlaced(Guid subconPackingListId)
        {
            SubconPackingListId = subconPackingListId;
        }

        public Guid SubconPackingListId { get; }
    }
}
