using Infrastructure.Domain.Events;
using Manufactures.Domain.GarmentAvalProducts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnGarmentAvalProductCreatedHandler : IDomainEventHandler<OnEntityCreated<GarmentAvalProduct>>
    {
        public Task Handle(OnEntityCreated<GarmentAvalProduct> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}