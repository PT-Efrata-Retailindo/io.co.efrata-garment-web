using Infrastructure.Domain.Events;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentPreparings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentPreparingItemUpdatedHandler : IDomainEventHandler<OnEntityUpdated<GarmentPreparingItem>>
    {
        public Task Handle(OnEntityUpdated<GarmentPreparingItem> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}