using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSubcon
{
    public class OnGarmentSubconContractPlaced : IGarmentSubconContractEvent
    {
        public OnGarmentSubconContractPlaced(Guid identity)
        {
            OnGarmentSubconContractId = identity;
        }
        public Guid OnGarmentSubconContractId { get; }
    }
}

