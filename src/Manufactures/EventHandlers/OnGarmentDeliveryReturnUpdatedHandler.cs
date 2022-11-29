using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentDeliveryReturns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentDeliveryReturnUpdatedHandler : IDomainEventHandler<OnEntityUpdated<GarmentDeliveryReturn>>
    {
        public Task Handle(OnEntityUpdated<GarmentDeliveryReturn> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}