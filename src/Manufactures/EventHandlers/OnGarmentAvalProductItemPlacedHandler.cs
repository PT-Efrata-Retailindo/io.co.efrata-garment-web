using Manufactures.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentAvalProductItemPlacedHandler : IGarmentAvalProductItemEventHandler<OnGarmentAvalProductItemPlaced>
    {
        public Task Handle(OnGarmentAvalProductItemPlaced notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}