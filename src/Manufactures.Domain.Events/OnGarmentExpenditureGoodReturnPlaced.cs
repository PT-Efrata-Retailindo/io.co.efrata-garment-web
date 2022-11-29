using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
    public class OnGarmentExpenditureGoodReturnPlaced : IGarmentExpenditureGoodReturnEvent
    {
        public OnGarmentExpenditureGoodReturnPlaced(Guid identity)
        {
            OnGarmentExpenditureGoodReturnId = identity;
        }
        public Guid OnGarmentExpenditureGoodReturnId { get; }
    }
}
