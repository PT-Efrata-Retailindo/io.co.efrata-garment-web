using Infrastructure.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample.SampleDeliveryReturns
{
    public interface IGarmentSampleDeliveryReturnEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IGarmentSampleDeliveryReturnEvent
    {
    }
}
