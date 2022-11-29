using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentSewingOutPlaced : IGarmentSewingOutEvent
    {
        public OnGarmentSewingOutPlaced(Guid garmentSewingOutId)
        {
            GarmentSewingOutId = garmentSewingOutId;
        }
        public Guid GarmentSewingOutId { get; }
    }
}
