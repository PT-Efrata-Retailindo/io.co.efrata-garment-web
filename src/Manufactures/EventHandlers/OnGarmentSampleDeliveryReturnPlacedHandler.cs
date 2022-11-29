using Manufactures.Domain.Events.GarmentSample.SampleDeliveryReturns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentSampleDeliveryReturnPlacedHandler : IGarmentSampleDeliveryReturnEventHandler<OnGarmentSampleDeliveryReturnPlaced>
    {
        public Task Handle(OnGarmentSampleDeliveryReturnPlaced notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
