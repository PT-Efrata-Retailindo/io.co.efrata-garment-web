using Infrastructure.Domain.Events;

namespace Manufactures.Domain.Events
{
    public interface IGarmentSewingDOItemEventHandler<TEvent> : IDomainEventHandler<TEvent> where TEvent : IGarmentSewingDOItemEvent
    {
    }
}