using Infrastructure.Domain.Events;

namespace Manufactures.Domain.Events
{
    public interface IGarmentDeliveryReturnItemEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IGarmentDeliveryReturnItemEvent
    {
    }
}