using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentSampleAvalProductUpdatedHandler : IDomainEventHandler<OnEntityUpdated<GarmentSampleAvalProduct>>
    {
        public Task Handle(OnEntityUpdated<GarmentSampleAvalProduct> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
