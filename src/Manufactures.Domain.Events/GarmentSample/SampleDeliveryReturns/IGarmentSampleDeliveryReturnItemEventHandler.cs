using Infrastructure.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample.SampleDeliveryReturns
{
    public interface IGarmentSampleDeliveryReturnItemEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IGarmentSampleDeliveryReturnItemEvent
    {
    }
}
