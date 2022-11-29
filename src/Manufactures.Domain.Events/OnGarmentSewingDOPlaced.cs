using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentSewingDOPlaced : IGarmentSewingDOEvent
    {
        public OnGarmentSewingDOPlaced(Guid garmentSewingDOId)
        {
            GarmentSewingDOId = garmentSewingDOId;
        }
        public Guid GarmentSewingDOId { get; }
    }
}