using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentSewingDOItemPlaced : IGarmentSewingDOItemEvent
    {
        public OnGarmentSewingDOItemPlaced(Guid garmentSewingDOItemId)
        {
            GarmentSewingDOItemId = garmentSewingDOItemId;
        }
        public Guid GarmentSewingDOItemId { get; }
    }
}