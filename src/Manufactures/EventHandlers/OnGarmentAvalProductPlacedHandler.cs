using Manufactures.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentAvalProductPlacedHandler : IGarmentAvalProductEventHandler<OnGarmentAvalProductPlaced>
    {
        public Task Handle(OnGarmentAvalProductPlaced notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}