using Manufactures.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentPreparingItemPlacedHandler : IGarmentPreparingItemEventHandler<OnGarmentPreparingItemPlaced>
    {
        public Task Handle(OnGarmentPreparingItemPlaced notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}