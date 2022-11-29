using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentSampleDeliveryReturnCreatedHandler : IDomainEventHandler<OnEntityCreated<GarmentSampleDeliveryReturn>>
    {
        public Task Handle(OnEntityCreated<GarmentSampleDeliveryReturn> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
