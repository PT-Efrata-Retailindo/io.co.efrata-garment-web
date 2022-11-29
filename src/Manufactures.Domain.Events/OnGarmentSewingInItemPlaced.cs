using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentSewingInItemPlaced : IGarmentSewingDOItemEvent
    {
        public OnGarmentSewingInItemPlaced(Guid garmentSewingInItemId)
        {
            GarmentSewingInItemId = garmentSewingInItemId;
        }
        public Guid GarmentSewingInItemId { get; }
    }
}