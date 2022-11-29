using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentSampleDeliveryReturnUpdatedHandler : IDomainEventHandler<OnEntityUpdated<GarmentSampleDeliveryReturn>>
    {
        public Task Handle(OnEntityUpdated<GarmentSampleDeliveryReturn> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
