using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentSewingInPlaced : IGarmentSewingInEvent
    {
        public OnGarmentSewingInPlaced(Guid garmentSewingInId)
        {
            GarmentSewingInId = garmentSewingInId;
        }
        public Guid GarmentSewingInId { get; }
    }
}