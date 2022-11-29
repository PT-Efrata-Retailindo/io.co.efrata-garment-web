using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample
{
    public class OnGarmentSampleExpenditureGoodPlaced : IGarmentSampleExpenditureGoodEvent
    {
        public OnGarmentSampleExpenditureGoodPlaced(Guid identity)
        {
            OnGarmentSampleExpenditureGoodId = identity;
        }
        public Guid OnGarmentSampleExpenditureGoodId { get; }
    }
}
