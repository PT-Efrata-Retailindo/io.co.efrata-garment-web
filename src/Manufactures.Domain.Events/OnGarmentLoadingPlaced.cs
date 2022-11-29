using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentLoadingPlaced : IGarmentLoadingEvent
    {
        public OnGarmentLoadingPlaced(Guid identity)
        {
            OnGarmentLoadingId = identity;
        }
        public Guid OnGarmentLoadingId { get; }
    }
}
