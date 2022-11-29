using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentDeliveryReturns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentDeliveryReturnCreatedHandler : IDomainEventHandler<OnEntityCreated<GarmentDeliveryReturn>>
    {
        public Task Handle(OnEntityCreated<GarmentDeliveryReturn> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}