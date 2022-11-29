using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentSampleSewingInPlaced : IGarmentSampleSewingInEvent
    {
        public OnGarmentSampleSewingInPlaced(Guid identity)
        {
            OnGarmentSampleSewingInId = identity;
        }
        public Guid OnGarmentSampleSewingInId { get; }
    }
}
