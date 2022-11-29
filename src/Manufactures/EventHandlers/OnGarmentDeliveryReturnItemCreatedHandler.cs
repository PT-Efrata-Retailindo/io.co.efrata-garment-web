using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentDeliveryReturns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentDeliveryReturnItemCreatedHandler : IDomainEventHandler<OnEntityCreated<GarmentDeliveryReturnItem>>
    {
        public Task Handle(OnEntityCreated<GarmentDeliveryReturnItem> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}