using Infrastructure.Domain.Events;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentDeliveryReturns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentDeliveryReturnItemUpdatedHandler : IDomainEventHandler<OnEntityUpdated<GarmentDeliveryReturnItem>>
    {
        public Task Handle(OnEntityUpdated<GarmentDeliveryReturnItem> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}