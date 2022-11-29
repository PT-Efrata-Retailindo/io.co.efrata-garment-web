using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentAvalProducts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentAvalProductUpdatedHandler : IDomainEventHandler<OnEntityUpdated<GarmentAvalProduct>>
    {
        public Task Handle(OnEntityUpdated<GarmentAvalProduct> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}