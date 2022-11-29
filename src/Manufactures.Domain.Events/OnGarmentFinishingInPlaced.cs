using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentFinishingInPlaced : IGarmentFinishingInEvent
    {
        public OnGarmentFinishingInPlaced(Guid identity)
        {
            OnGarmentFinishingInId = identity;
        }
        public Guid OnGarmentFinishingInId { get; }
    }
}
