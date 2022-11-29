using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentSampleDeliveryReturnItemUpdatedHandler : IDomainEventHandler<OnEntityUpdated<GarmentSampleDeliveryReturnItem>>
    {
        public Task Handle(OnEntityUpdated<GarmentSampleDeliveryReturnItem> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
