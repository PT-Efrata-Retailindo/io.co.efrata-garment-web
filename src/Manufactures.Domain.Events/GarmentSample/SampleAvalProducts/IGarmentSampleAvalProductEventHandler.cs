using Infrastructure.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events.GarmentSample.SampleAvalProducts
{
    public interface IGarmentSampleAvalProductEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IGarmentSampleAvalProductEvent
    {
    }
}
