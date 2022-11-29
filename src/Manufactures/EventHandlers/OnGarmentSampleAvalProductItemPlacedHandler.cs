using Manufactures.Domain.Events.GarmentSample.SampleAvalProducts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentSampleAvalProductItemPlacedHandler : IGarmentSampleAvalProductItemEventHandler<OnGarmentSampleAvalProductItemPlaced>
    {
        public Task Handle(OnGarmentSampleAvalProductItemPlaced notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
