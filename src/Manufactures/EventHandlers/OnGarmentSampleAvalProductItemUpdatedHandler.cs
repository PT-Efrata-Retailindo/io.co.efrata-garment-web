using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentSampleAvalProductItemUpdatedHandler : IDomainEventHandler<OnEntityUpdated<GarmentSampleAvalProductItem>>
    {
        public Task Handle(OnEntityUpdated<GarmentSampleAvalProductItem> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
