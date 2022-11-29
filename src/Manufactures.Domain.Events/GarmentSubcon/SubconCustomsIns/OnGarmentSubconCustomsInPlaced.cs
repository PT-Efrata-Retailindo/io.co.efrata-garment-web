using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSubcon.SubconCustomsIns
{
    public class OnGarmentSubconCustomsInPlaced : ISubconCustomsInEvent
    {
        public OnGarmentSubconCustomsInPlaced(Guid identity)
        {
            OnSubconCustomsInId = identity;
        }
        public Guid OnSubconCustomsInId { get; }
    }
}