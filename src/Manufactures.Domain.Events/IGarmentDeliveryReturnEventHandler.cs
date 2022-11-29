using Infrastructure.Domain.Events;

namespace Manufactures.Domain.Events
{
    public interface IGarmentDeliveryReturnEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IGarmentDeliveryReturnEvent
    {
    }
}