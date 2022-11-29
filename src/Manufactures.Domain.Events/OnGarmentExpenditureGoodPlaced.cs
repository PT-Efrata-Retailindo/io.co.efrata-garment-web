using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentExpenditureGoodPlaced : IGarmentExpenditureGoodEvent
    {
        public OnGarmentExpenditureGoodPlaced(Guid identity)
        {
            OnGarmentExpenditureGoodId = identity;
        }
        public Guid OnGarmentExpenditureGoodId { get; }
    }
}
