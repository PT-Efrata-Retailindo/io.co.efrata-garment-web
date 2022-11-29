using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentComodityPricePlaced : IGarmentComodityPriceEvent
    {
        public OnGarmentComodityPricePlaced(Guid identity)
        {
            OnGarmentComodityPriceId = identity;
        }
        public Guid OnGarmentComodityPriceId { get; }
    }
}
