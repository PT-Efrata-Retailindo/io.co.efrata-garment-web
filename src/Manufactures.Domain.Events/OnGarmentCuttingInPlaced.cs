using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentCuttingInPlaced : IGarmentCuttingInEvent
    {
        public OnGarmentCuttingInPlaced(Guid identity)
        {
            OnGarmentCuttingInId = identity;
        }
        public Guid OnGarmentCuttingInId { get; }
    }
}