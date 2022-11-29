using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentAvalProducts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentAvalProductItemCreatedHandler : IDomainEventHandler<OnEntityCreated<GarmentAvalProductItem>>
    {
        public Task Handle(OnEntityCreated<GarmentAvalProductItem> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}