using Manufactures.Domain.Events.GarmentSample.SampleAvalProducts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentSampleAvalProductPlacedHandler : IGarmentSampleAvalProductEventHandler<OnGarmentSampleAvalProductPlaced>
    {
        public Task Handle(OnGarmentSampleAvalProductPlaced notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
