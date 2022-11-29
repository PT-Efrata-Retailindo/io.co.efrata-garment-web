using Manufactures.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Manufactures.EventHandlers
{
    class OnGarmentDeliveryReturnItemPlacedHandler : IGarmentDeliveryReturnItemEventHandler<OnGarmentDeliveryReturnItemPlaced>
    {
        public Task Handle(OnGarmentDeliveryReturnItemPlaced notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}