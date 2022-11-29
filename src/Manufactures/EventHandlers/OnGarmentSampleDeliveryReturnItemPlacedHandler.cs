using Manufactures.Domain.Events.GarmentSample.SampleDeliveryReturns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentSampleDeliveryReturnItemPlacedHandler : IGarmentSampleDeliveryReturnItemEventHandler<OnGarmentSampleDeliveryReturnItemPlaced>
    {
        public Task Handle(OnGarmentSampleDeliveryReturnItemPlaced notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
