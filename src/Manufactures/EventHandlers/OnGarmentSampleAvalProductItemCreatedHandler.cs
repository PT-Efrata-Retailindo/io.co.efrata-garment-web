using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentSampleAvalProductItemCreatedHandler : IDomainEventHandler<OnEntityCreated<GarmentSampleAvalProductItem>>
    {
        public Task Handle(OnEntityCreated<GarmentSampleAvalProductItem> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
